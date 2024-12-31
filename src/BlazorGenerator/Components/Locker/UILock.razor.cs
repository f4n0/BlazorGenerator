using BlazorGenerator.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorGenerator.Components.Locker
{
  public partial class UILock
  {
    [Inject]
    LockUIService? LockUIService { get; set; }

    bool _showLock = false;

    protected override void OnInitialized()
    {
      LockUIService!.OnChange += UpdateProgress;
    }

    private void UpdateProgress(bool show)
    {
      _showLock = show;
      StateHasChanged();
    }

    public void Dispose()
    {
      LockUIService!.OnChange -= UpdateProgress;
    }
  }
}
