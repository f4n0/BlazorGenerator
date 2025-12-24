using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Filled;

namespace BlazorEngine.Services
{
  public class BackgroundExecutor : IDisposable
  {
    public const string SectionName = "BLAZORENGINE_BACKGROUND_EXECUTOR";

    private class EnqueueItem(string taskTitle, Message? message, Func<Task> action)
    {
      public string TaskTitle { get; set; } = taskTitle;
      public Message? Message { get; set; } = message;
      public Func<Task> Action { get; set; } = action;

      public string BuildTitle(string message) => $"{TaskTitle} - {message}";
    }

    private readonly ConcurrentQueue<EnqueueItem> _actionQueue = new();
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(0);
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly IMessageService _messageService;
    private bool _isProcessing = false;
    private Task? _processingTask;
    private readonly IToastService _toastService;

    public BackgroundExecutor(IMessageService messageService, IToastService  toastService)
    {
      _toastService = toastService;
      _messageService = messageService;
      StartProcessingQueue();
    }

    public void QueueAction(string taskTitle, Func<Task> action)
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
            try
            {
              item.Message = ShowNotification(item.BuildTitle("Executing action"), MessageIntent.Custom, item.Message);
              await item.Action();
              item.Message = ShowNotification(item.BuildTitle("Action executed successfully"), MessageIntent.Success, item.Message);
            }
            catch (Exception ex)
            {
              item.Message = ShowNotification(item.BuildTitle("Action execution failed"), MessageIntent.Error, item.Message);
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

    private Message ShowNotification(string message, MessageIntent intent = MessageIntent.Info, Message? replace = null)
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
      });

    }

    public void Dispose()
    {
      if (_isProcessing)
      {
        _cancellationTokenSource.Cancel();
        _processingTask?.Wait(TimeSpan.FromSeconds(1));
      }

      _cancellationTokenSource.Dispose();
      _semaphore.Dispose();

    }
  }
}