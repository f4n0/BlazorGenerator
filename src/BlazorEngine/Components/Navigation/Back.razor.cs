using BlazorEngine.Components.Base;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorEngine.Components.Navigation
{
  public partial class Back : BlazorEngineComponentBase
  {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0039:Use local function", Justification = "<Pending>")]
    protected override void OnParametersSet()
    {
      EventHandler<LocationChangedEventArgs> onLocationChange = (_, _) => InvokeAsync(() => StateHasChanged());

      NavManager.LocationChanged += onLocationChange;

      UIServices.KeyCodeService.RegisterListener(OnKeyDownAsync);
    }


    private async Task OnKeyDownAsync(FluentKeyCodeEventArgs args)
    {
      if ((args.Key == KeyCode.Left && args.AltKey))
      {
        if (NavManager.ToBaseRelativePath(NavManager.Uri) != "")
          await JSRuntime.InvokeVoidAsync("history.back", ComponentDetached);
      }

    }
  }
}
