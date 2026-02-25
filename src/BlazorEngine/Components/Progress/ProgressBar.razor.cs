using BlazorEngine.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorEngine.Components.Progress;

public partial class ProgressBar : IDisposable
{
  private bool _showProgress;
  [Inject] private ProgressService? ProgressService { get; set; }

  public void Dispose()
  {
    ProgressService!.OnChange -= UpdateProgress;
  }

  protected override void OnInitialized()
  {
    ProgressService!.OnChange += UpdateProgress;
  }

  private void UpdateProgress(bool show)
  {
    _showProgress = show;
    InvokeAsync(() => StateHasChanged());
  }
}