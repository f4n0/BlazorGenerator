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

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();

    public void SendLogMessage(string message, LogType logType = LogType.Info)
    {
      Logs.Add((message, logType));
      NotifyStateChanged();
    }



    public List<(string, LogType)> Logs = new List<(string, LogType)>();
  }
}
