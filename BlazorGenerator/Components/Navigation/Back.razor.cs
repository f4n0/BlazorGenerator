using BlazorGenerator.Components.Base;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorGenerator.Components.Navigation
{
  public partial class Back : BlazorGeneratorComponentBase
  {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0039:Use local function", Justification = "<Pending>")]
    protected override void OnParametersSet()
    {
      var parts = NavManager!.ToBaseRelativePath(NavManager.Uri);

      EventHandler<LocationChangedEventArgs> OnLocationChange = (sender, args) => StateHasChanged();

      NavManager!.LocationChanged += OnLocationChange;

      UIServices.KeyCodeService.RegisterListener(OnKeyDownAsync);
    }


    private async Task OnKeyDownAsync(FluentKeyCodeEventArgs args)
    {
      if ((args.Key == KeyCode.Left && args.AltKey))
      {
        if (NavManager?.ToBaseRelativePath(NavManager.Uri.ToString()) != "")
          await JSRuntime.InvokeVoidAsync("history.back");
      }

    }
  }
}
