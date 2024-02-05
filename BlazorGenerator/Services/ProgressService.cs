using BlazorGenerator.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Services
{
  public class ProgressService
  {
    internal event Action<bool>? OnChange;

    private void NotifyStateChanged(bool val) => OnChange?.Invoke(val);

    public void StartProgress()
    {
      NotifyStateChanged(true);
    }

    public void StopProgress()
    {
      NotifyStateChanged(false);
    }
  }
}
