using BlazorEngine.Components.Background;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Filled;

namespace BlazorEngine.Services
{
  public class BackgroundExecutor : IDisposable
  {
    public const string SectionName = "BLAZORENGINE_BACKGROUND_EXECUTOR";

    public IDialogReference? PanelExecutorRef { get; set; }

    private class EnqueueItem(string taskTitle, Message? message, Func<ILogger, Task> action)
    {
      public string TaskTitle { get; set; } = taskTitle;
      public Message? Message { get; set; } = message;
      public Func<ILogger, Task> Action { get; set; } = action;

      public string BuildTitle(string message) => $"{TaskTitle} - {message}";
    }

    private readonly ConcurrentQueue<EnqueueItem> _actionQueue = new();
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0);
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly IMessageService _messageService;
    private bool _isProcessing = false;
    private Task? _processingTask;
    private readonly IToastService _toastService;
    private readonly IDialogService _dialogService;

    public BackgroundExecutor(IMessageService messageService, IToastService toastService, IDialogService dialogService)
    {
      _toastService = toastService;
      _messageService = messageService;
      _dialogService = dialogService;
      StartProcessingQueue();
    }

    public void QueueAction(string taskTitle, Func<ILogger, Task> action)
    {
      _actionQueue.Enqueue(new EnqueueItem(taskTitle, ShowNotification(taskTitle + " - Enqueued"), action));
      _semaphore.Release();
    }

    private void StartProcessingQueue()
    {
      _isProcessing = true;
      _processingTask = Task.Run(ProcessQueueAsync, _cancellationTokenSource.Token);
    }

    private async Task ProcessQueueAsync()
    {
      try
      {
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
          await _semaphore.WaitAsync(_cancellationTokenSource.Token);

          if (_actionQueue.TryDequeue(out EnqueueItem? item))
          {
            var logger = new ActionLogger();
            try
            {
              item.Message = ShowNotification(item.BuildTitle("Executing action"), MessageIntent.Custom, item.Message,
                logger);
              await item.Action(logger);
              item.Message = ShowNotification(item.BuildTitle("Action executed successfully"), MessageIntent.Success,
                item.Message, logger);
            }
            catch (Exception ex)
            {
              logger.LogError(ex, "Action execution failed");
              item.Message = ShowNotification(item.BuildTitle("Action execution failed"), MessageIntent.Error,
                item.Message, logger);
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
        ShowNotification($"Queue processing error: {ex.Message}", MessageIntent.Error);
      }
      finally
      {
        _isProcessing = false;
      }
    }

    private Message ShowNotification(string message, MessageIntent intent = MessageIntent.Info, Message? replace = null,
      ActionLogger? logger = null)
    {
      if (replace != null)
      {
        _messageService.Remove(replace);
      }

      if (intent == MessageIntent.Success)
      {
        _toastService.ShowSuccess(message);
      }

      return _messageService.ShowMessageBar(options =>
      {
        options.Icon = intent == MessageIntent.Custom ? new Size20.PersonRunning() : null;
        options.Timeout = intent == MessageIntent.Success ? 10000 : null;
        options.Intent = intent;
        options.Title = "Background Executor";
        options.Body = message;
        options.Timestamp = DateTime.Now;
        options.Section = SectionName;

        if (logger != null)
        {
          options.PrimaryAction = new ActionButton<Message>()
          {
            Text = "View Logs",
            OnClick = async (_) =>
            {
              if (PanelExecutorRef != null)
                await PanelExecutorRef.CloseAsync();
              await _dialogService.ShowDialogAsync(typeof(ActionLogPanel), logger, new DialogParameters()
              {
                Alignment = HorizontalAlignment.Right,
                Title = "Action Logs",
                PrimaryAction = null,
                SecondaryAction = null,
                ShowDismiss = true,
                Width = "60% ",
                Height = "fit-content",
              });
            }
          };
        }
      });
    }

    public void Dispose()
    {
      ShutdownAsync(TimeSpan.FromSeconds(10)).GetAwaiter().GetResult();
      _cancellationTokenSource.Dispose();
      _semaphore.Dispose();
    }

    public async Task ShutdownAsync(TimeSpan timeout)
    {
      // Stop accepting new actions
      _isProcessing = false;
      await _cancellationTokenSource.CancelAsync();
      _semaphore.Release(); // Unblock if waiting

      if (_processingTask != null)
      {
        var completed = await Task.WhenAny(_processingTask, Task.Delay(timeout));
      }
    }
  }
}