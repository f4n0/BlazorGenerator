using BlazorGenerator.Components.Base;
using Microsoft.AspNetCore.Components.Routing;

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
    }
  }
}
