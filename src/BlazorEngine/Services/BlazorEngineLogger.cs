using BlazorEngine.Enum;

namespace BlazorEngine.Services
{
  public class BlazorEngineLogger
  {
    public event Action<string, LogType>? OnLogWrite;
    internal event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();

    public void SendLogMessage(string message, LogType logType = LogType.Info)
    {
      Logs.Add((FormatLogMessage(message), logType));
      NotifyStateChanged();

      OnLogWrite?.Invoke(message, logType);
    }

    public List<(string, LogType)> Logs = [];


    string FormatLogMessage(string message)
    {
      return DateTime.Now + " - " + message;
    }
  }
}
