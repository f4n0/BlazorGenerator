using BlazorEngine.Enum;
using BlazorEngine.Models;

namespace BlazorEngine.Services;

public class BlazorEngineLogger
{
  public const int MaxLogEntries = 2000;

  public CircularLogBuffer Logs = new(MaxLogEntries);
  public event Action<string, LogType>? OnLogWrite;
  internal event Action? OnChange;

  private void NotifyStateChanged()
  {
    OnChange?.Invoke();
  }

  public void SendLogMessage(string message, LogType logType = LogType.Info)
  {
    Logs.Add(FormatLogMessage(message), logType);
    NotifyStateChanged();

    OnLogWrite?.Invoke(message, logType);
  }


  private string FormatLogMessage(string message)
  {
    return DateTime.Now + " - " + message;
  }
}