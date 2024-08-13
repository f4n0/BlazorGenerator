using BlazorGenerator.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorGenerator.Components.Locker
{
  public partial class UILock
  {
    [Inject]
    LockUIService? LockUIService { get; set; }

    bool ShowLock = false;

    protected override void OnInitialized()
    {
      LockUIService!.OnChange += UpdateProgress;
    }

    private void UpdateProgress(bool show)
    {
      ShowLock = show;
      StateHasChanged();
    }

    public void Dispose()
    {
      LockUIService!.OnChange -= UpdateProgress;
    }
  }
}
