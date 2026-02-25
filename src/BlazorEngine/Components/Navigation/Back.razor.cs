using BlazorEngine.Components.Base;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorEngine.Components.Navigation;

public partial class Back : BlazorEngineComponentBase
{
  private EventHandler<LocationChangedEventArgs>? _onLocationChange;

  protected override void OnInitialized()
  {
    _onLocationChange = (_, _) => InvokeAsync(() => StateHasChanged());
    NavManager.LocationChanged += _onLocationChange;
    UIServices.KeyCodeService.RegisterListener(OnKeyDownAsync);
  }

  private async Task OnKeyDownAsync(FluentKeyCodeEventArgs args)
  {
    if (args.Key == KeyCode.Left && args.AltKey)
      if (NavManager.ToBaseRelativePath(NavManager.Uri) != "")
        await JSRuntime.InvokeVoidAsync("history.back", ComponentDetached);
  }

  public override void InternalDispose()
  {
    if (_onLocationChange != null)
      NavManager.LocationChanged -= _onLocationChange;
    UIServices.KeyCodeService.UnregisterListener(OnKeyDownAsync);
    base.InternalDispose();
  }

  public override ValueTask InternalDisposeAsync()
  {
    if (_onLocationChange != null)
      NavManager.LocationChanged -= _onLocationChange;
    UIServices.KeyCodeService.UnregisterListener(OnKeyDownAsync);
    return base.InternalDisposeAsync();
  }
}