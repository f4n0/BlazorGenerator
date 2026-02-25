using BlazorEngine.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorEngine.Components.Locker;

public partial class UILock : IDisposable
{
  private bool _showLock;
  [Inject] private LockUIService? LockUIService { get; set; }

  public void Dispose()
  {
    LockUIService!.OnChange -= UpdateProgress;
  }

  protected override void OnInitialized()
  {
    LockUIService!.OnChange += UpdateProgress;
  }

  private void UpdateProgress(bool show)
  {
    _showLock = show;
    InvokeAsync(() => StateHasChanged());
  }
}