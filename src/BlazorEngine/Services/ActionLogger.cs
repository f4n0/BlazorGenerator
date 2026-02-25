using Microsoft.Extensions.Logging;

namespace BlazorEngine.Services;

public class ActionLogEntry
{
  public DateTime Timestamp { get; init; }
  public LogLevel LogLevel { get; init; }
  public string Message { get; init; } = string.Empty;
}

public class ActionLogger : ILogger
{
  private readonly List<ActionLogEntry> _entries = [];
  private readonly object _lock = new();

  public IReadOnlyList<ActionLogEntry> Entries
  {
    get
    {
      lock (_lock)
      {
        return _entries.ToArray();
      }
    }
  }

  public IDisposable? BeginScope<TState>(TState state) where TState : notnull
  {
    return null;
  }

  public bool IsEnabled(LogLevel logLevel)
  {
    return true;
  }

  public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
    Func<TState, Exception?, string> formatter)
  {
    if (!IsEnabled(logLevel))
      return;

    var message = formatter(state, exception);
    if (exception != null)
      message += Environment.NewLine + exception;

    var entry = new ActionLogEntry
    {
      Timestamp = DateTime.UtcNow,
      LogLevel = logLevel,
      Message = message
    };

    lock (_lock)
    {
      _entries.Add(entry);
    }

    OnChange?.Invoke();
  }

  public event Action? OnChange;

  public void Clear()
  {
    lock (_lock)
    {
      _entries.Clear();
    }

    OnChange?.Invoke();
  }
}