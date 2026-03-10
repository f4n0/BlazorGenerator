using System.Collections.Concurrent;
using BlazorEngine.Components.Background;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Filled;

namespace BlazorEngine.Services;

public class BackgroundExecutor : IAsyncDisposable
{
  public const string SectionName = "BLAZORENGINE_BACKGROUND_EXECUTOR";
  private const int DefaultShutdownTimeoutSeconds = 10;
  private const int SuccessNotificationTimeout = 10000;
  private const string DialogWidth = "60%";

  private readonly ConcurrentQueue<EnqueueItem> _actionQueue = new();
  private readonly CancellationTokenSource _cancellationTokenSource = new();
  private readonly IDialogService _dialogService;
  private readonly IMessageService _messageService;
  private readonly Task _processingTask;
  private readonly SemaphoreSlim _semaphore = new(0);
  private readonly IToastService _toastService;
  private bool _disposed;

  public BackgroundExecutor(IMessageService messageService, IToastService toastService, IDialogService dialogService)
  {
    _toastService = toastService;
    _messageService = messageService;
    _dialogService = dialogService;
    _processingTask = Task.Run(ProcessQueueAsync);
  }

  public IDialogReference? PanelExecutorRef { get; set; }

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
          var logger = new ActionLogger();
          try
          {
            item.Message = ShowNotification(item.BuildTitle("Executing action"), item, MessageIntent.Custom,
              logger);
            await item.Action(logger, item.CancellationToken.Token);
            item.Message = ShowNotification(item.BuildTitle("Action executed successfully"), item, MessageIntent.Success,
               logger);
          }
          catch (TaskCanceledException ex)
          {

            logger.LogError("The task was cancelled by the user");
            item.Message = ShowNotification(item.BuildTitle("Action execution stopped"), item, MessageIntent.Error,
               logger);
          }
          catch (OperationCanceledException ex)
          {
            logger.LogError("The task was cancelled by the user");
            item.Message = ShowNotification(item.BuildTitle("Action execution stopped"), item, MessageIntent.Error,
               logger);
          }
          catch (Exception ex)
          {
            logger.LogError(ex, "Action execution failed");
            item.Message = ShowNotification(item.BuildTitle("Action execution failed"), item, MessageIntent.Error,
               logger);
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
      ShowNotification($"Queue processing error: {ex.Message}",null, MessageIntent.Error);
    }
  }

  private Message ShowNotification(string message, EnqueueItem? item, MessageIntent intent = MessageIntent.Info,
    ActionLogger? logger = null)
  {
    if (item?.Message != null) _messageService.Remove(item.Message);

    if (intent == MessageIntent.Success) _toastService.ShowSuccess(message);

    return _messageService.ShowMessageBar(options =>
    {
      options.Icon = intent == MessageIntent.Custom ? new Size20.PersonRunning() : null;
      options.Timeout = intent == MessageIntent.Success ? SuccessNotificationTimeout : null;
      options.Intent = intent;
      options.Title = "Background Executor";
      options.Body = message;
      options.Timestamp = DateTime.UtcNow;
      options.Section = SectionName;
      options.AllowDismiss = intent != MessageIntent.Custom;

      if (logger != null)
        options.PrimaryAction = new ActionButton<Message>
        {
          Text = "View Logs",
          OnClick = async _ =>
          {
            if (PanelExecutorRef != null)
              await PanelExecutorRef.CloseAsync();
            await _dialogService.ShowDialogAsync(typeof(ActionLogPanel), logger, new DialogParameters
            {
              Alignment = HorizontalAlignment.Right,
              Title = "Action Logs",
              PrimaryAction = null,
              SecondaryAction = null,
              ShowDismiss = true,
              Width = DialogWidth,
              Height = "fit-content"
            });
          }
        };

      if (intent == MessageIntent.Custom)
        options.SecondaryAction = new ActionButton<Message>
        {
          Text = "Stop",
          OnClick = async _ =>
          {
            await item!.CancellationToken.CancelAsync();
          }

        };

    });
  }

  private async Task ShutdownAsync(TimeSpan timeout)
  {
    await _cancellationTokenSource.CancelAsync();
    _semaphore.Release(); // Unblock if waiting

    await Task.WhenAny(_processingTask, Task.Delay(timeout));
  }

  private class EnqueueItem(string taskTitle, Message? message, Func<ILogger, CancellationToken, Task> action)
  {
    public string TaskTitle { get; } = taskTitle;
    public Message? Message { get; set; } = message;
    public Func<ILogger, CancellationToken, Task> Action { get; } = action;

    public CancellationTokenSource CancellationToken = new();

    public string BuildTitle(string message)
    {
      return $"{TaskTitle} - {message}";
    }
  }
}