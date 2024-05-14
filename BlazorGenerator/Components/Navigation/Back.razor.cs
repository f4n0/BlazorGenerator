using BlazorGenerator.Components.Base;
using Microsoft.AspNetCore.Components.Routing;

namespace BlazorGenerator.Components.Navigation
{
  public partial class Back : BlazorgenComponentBase
  {
    protected override void OnParametersSet()
    {
      var parts = NavManager!.ToBaseRelativePath(NavManager.Uri);

      EventHandler<LocationChangedEventArgs> OnLocationChange = (sender, args) => StateHasChanged();

      NavManager!.LocationChanged += OnLocationChange;
    }
  }
}
