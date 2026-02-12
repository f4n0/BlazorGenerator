using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace BlazorEngine.Services
{
  public class ActionLogEntry
  {
    public DateTime Timestamp { get; set; }
    public LogLevel LogLevel { get; set; }
    public string Message { get; set; } = string.Empty;
  }

  public class ActionLogger : ILogger
  {
    private readonly object _lock = new();
    private readonly List<ActionLogEntry> _entries = [];

    public event Action? OnChange;

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

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
      if (!IsEnabled(logLevel))
        return;

      var message = formatter(state, exception);
      if (exception != null)
        message += Environment.NewLine + exception;

      var entry = new ActionLogEntry
      {
        Timestamp = DateTime.Now,
        LogLevel = logLevel,
        Message = message
      };

      lock (_lock)
      {
        _entries.Add(entry);
      }

      OnChange?.Invoke();
    }

    public void Clear()
    {
      lock (_lock)
      {
        _entries.Clear();
      }
      OnChange?.Invoke();
    }
  }
}
