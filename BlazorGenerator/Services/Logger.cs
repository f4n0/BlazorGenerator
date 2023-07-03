using BlazorGenerator.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Services
{
  public class BlazorGenLogger
  {
    private string? savedLogMessage;
    private LogType savedLogType;

    public string LogMessage
    {
      get => savedLogMessage ?? string.Empty;
    }
    public LogType LogType
    {
      get => savedLogType;
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();

    public void SendLogMessage(string message, LogType logType = LogType.Info)
    {
      savedLogMessage = message;
      savedLogType = logType;
       NotifyStateChanged();
    }
  }
}
