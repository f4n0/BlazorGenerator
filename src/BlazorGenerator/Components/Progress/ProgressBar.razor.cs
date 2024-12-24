using BlazorGenerator.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorGenerator.Components.Progress
{
  public partial class ProgressBar
  {
    [Inject]
    ProgressService? ProgressService { get; set; }

    bool ShowProgress = false;

    protected override void OnInitialized()
    {
      ProgressService!.OnChange += UpdateProgress;
    }

    private void UpdateProgress(bool show)
    {
      ShowProgress = show;
      StateHasChanged();
    }

    public void Dispose()
    {
      ProgressService!.OnChange -= UpdateProgress;
    }
  }
}
