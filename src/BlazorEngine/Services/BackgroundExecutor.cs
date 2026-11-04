using System.Collections.Concurrent;
using BlazorEngine.Components.Background;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Filled;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace BlazorEngine.Services;

public class BackgroundExecutor : IAsyncDisposable
{
  public const string SectionName = "BLAZORENGINE_BACKGROUND_EXECUTOR";
  private const int DefaultShutdownTimeoutSeconds = 10;
  private const int SuccessNotificationTimeout = 10000;

  private readonly ConcurrentQueue<EnqueueItem> _actionQueue = new();
  private readonly CancellationTokenSource _cancellationTokenSource = new();
  private readonly IDialogService _dialogService;
  private readonly INotificationService _notificationService;
  private readonly ILogger<BackgroundExecutor> _logger;
  private readonly ConcurrentDictionary<string, IMessageBarInstance> _notifications = new();
  private readonly object _notificationLock = new();
  private Task _notificationTask = Task.CompletedTask;
  private readonly Task _processingTask;
  private readonly SemaphoreSlim _semaphore = new(0);
  private EnqueueItem? _currentItem;
  private bool _disposed;

  public BackgroundExecutor(INotificationService notificationService, IDialogService dialogService,
    ILogger<BackgroundExecutor> logger)
  {
    _notificationService = notificationService;
    _logger = logger;
    _dialogService = dialogService;
    _processingTask = Task.Run(ProcessQueueAsync);
  }

  public IDialogInstance? PanelExecutorRef { get; set; }
  public int NotificationCount => _notifications.Count;
  public event Action? OnNotificationsChanged;

  public async ValueTask DisposeAsync()
  {
    if (_disposed)
      return;

    _disposed = true;
    await ShutdownAsync(TimeSpan.FromSeconds(DefaultShutdownTimeoutSeconds));
    _cancellationTokenSource.Dispose();
    _semaphore.Dispose();
  }

  public void QueueAction(string taskTitle, Func<ILogger, CancellationToken, Task> action)
  {
    ObjectDisposedException.ThrowIf(_disposed, this);
    _actionQueue.Enqueue(new EnqueueItem(taskTitle, ShowNotification(taskTitle + " - Enqueued", null), action));
    _semaphore.Release();
  }

  private async Task ProcessQueueAsync()
  {
    try
    {
      while (!_cancellationTokenSource.Token.IsCancellationRequested)
      {
        await _semaphore.WaitAsync(_cancellationTokenSource.Token);

        if (_actionQueue.TryDequeue(out var item))
        {
          _currentItem = item;
          var logger = new ActionLogger();
          try
          {
            item.Message = ShowNotification(item.BuildTitle("Executing action"), item, MessageBarIntent.Custom,
              logger);
            await item.Action(logger, item.CancellationToken.Token);
            item.Message = ShowNotification(item.BuildTitle("Action executed successfully"), item, MessageBarIntent.Success,
               logger);
          }
          catch (TaskCanceledException)
          {

            logger.LogError("The task was cancelled by the user");
            item.Message = ShowNotification(item.BuildTitle("Action execution stopped"), item, MessageBarIntent.Error,
               logger);
          }
          catch (OperationCanceledException)
          {
            logger.LogError("The task was cancelled by the user");
            item.Message = ShowNotification(item.BuildTitle("Action execution stopped"), item, MessageBarIntent.Error,
               logger);
          }
          catch (Exception ex)
          {
            logger.LogError(ex, "Action execution failed");
            item.Message = ShowNotification(item.BuildTitle("Action execution failed"), item, MessageBarIntent.Error,
               logger);
          }
          finally
          {
            _currentItem = null;
            item.Dispose();
          }
        }
      }
    }
    catch (OperationCanceledException)
    {
      // Expected when cancellation is requested
    }
    catch (Exception ex)
    {
      ShowNotification($"Queue processing error: {ex.Message}",null, MessageBarIntent.Error);
    }
  }

  private string ShowNotification(string message, EnqueueItem? item, MessageBarIntent intent = MessageBarIntent.Info,
    ActionLogger? logger = null)
  {
    var id = Identifier.NewId();
    var options = new MessageBarOptions
    {
      Id = id,
      Icon = intent == MessageBarIntent.Custom ? new Size20.PersonRunning() : null,
      Lifetime = intent == MessageBarIntent.Success ? TimeSpan.FromMilliseconds(SuccessNotificationTimeout) : null,
      Intent = intent,
      Title = "Background Executor",
      Message = message,
      TimeStamp = DateTime.UtcNow,
      Section = SectionName,
      AllowDismiss = intent != MessageBarIntent.Custom,
      OnStatusChange = OnNotificationStatusChanged,
      Parameters = new Dictionary<string, object?>
      {
        [nameof(BackgroundTaskMessage.Logger)] = logger,
        [nameof(BackgroundTaskMessage.Stop)] = intent == MessageBarIntent.Custom && item != null
          ? (Func<Task>)(() => StopActionAsync(item))
          : null
      }
    };
    lock (_notificationLock)
    {
      _notificationTask = PublishNotificationAsync(_notificationTask, options, item?.Message);
    }
    return id;
  }

  private async Task PublishNotificationAsync(Task previousNotification, MessageBarOptions options, string? previousId)
  {
    try
    {
      await previousNotification;
      if (previousId != null) await _notificationService.CloseAsync(previousId);
      await _notificationService.ShowMessageBarAsync<BackgroundTaskMessage>(options);
      if (options.Intent == MessageBarIntent.Success)
        await _notificationService.ShowSuccessToastAsync(options.Message ?? string.Empty);
    }
    catch (Exception exception)
    {
      _logger.LogError(exception, "Unable to publish background task notification.");
    }
  }

  private static async Task StopActionAsync(EnqueueItem item)
  {
    try
    {
      await item.CancellationToken.CancelAsync();
    }
    catch (ObjectDisposedException)
    {
      // The action completed while its running notification was being replaced.
    }
  }

  private void OnNotificationStatusChanged(MessageBarEventArgs args)
  {
    if (args.Status == MessageBarLifecycleStatus.Visible && args.Instance != null)
      _notifications[args.Id] = args.Instance;
    else if (args.Status == MessageBarLifecycleStatus.Unmounted)
      _notifications.TryRemove(args.Id, out _);
    OnNotificationsChanged?.Invoke();
  }

  public async Task OpenActionLogsAsync(ActionLogger logger)
  {
    if (PanelExecutorRef != null) await PanelExecutorRef.CancelAsync();
    var options = new DialogOptions
    {
      Width = UISizeTokens.ActionLogDialogWidth,
      Height = UISizeTokens.ActionLogDialogHeight,
      Parameters = new Dictionary<string, object?> { [nameof(ActionLogPanel.Content)] = logger }
    };
    options.Header.Title = "Action Logs";
    options.Footer.PrimaryAction.Visible = false;
    options.Footer.SecondaryAction.Visible = false;
    await _dialogService.ShowDialogAsync<ActionLogPanel>(options);
  }

  private async Task ShutdownAsync(TimeSpan timeout)
  {
    await _cancellationTokenSource.CancelAsync();

    if (_currentItem != null)
      _currentItem.CancellationToken.Cancel();

    while (_actionQueue.TryDequeue(out var item))
    {
      item.CancellationToken.Cancel();
      item.Dispose();
    }

    _semaphore.Release(); // Unblock if waiting

    await Task.WhenAny(_processingTask, Task.Delay(timeout));
  }

  private sealed class EnqueueItem(string taskTitle, string? message, Func<ILogger, CancellationToken, Task> action) : IDisposable
  {
    public string TaskTitle { get; } = taskTitle;
    public string? Message { get; set; } = message;
    public Func<ILogger, CancellationToken, Task> Action { get; } = action;

    public CancellationTokenSource CancellationToken = new();

    public string BuildTitle(string message)
    {
      return $"{TaskTitle} - {message}";
    }

    public void Dispose()
    {
      CancellationToken.Dispose();
    }
  }
}
