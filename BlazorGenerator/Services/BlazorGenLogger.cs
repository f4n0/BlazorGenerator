using BlazorGenerator.Enum;

namespace BlazorGenerator.Services
{
  public class BlazorGenLogger
  {
    public event Action<string, LogType>? OnLogWrite;
    internal event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();

    public void SendLogMessage(string message, LogType logType = LogType.Info)
    {
      Logs.Add((message, logType));
      NotifyStateChanged();

      OnLogWrite?.Invoke(message, logType);
    }

    public List<(string, LogType)> Logs = [];
  }
}
