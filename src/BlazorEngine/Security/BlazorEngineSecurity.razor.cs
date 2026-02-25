using BlazorEngine.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorEngine.Security;

public partial class BlazorEngineSecurity : ComponentBase
{
  [Parameter]
  public required RenderFragment ChildContent { get; set; }

  [Parameter]
  public required RouteData RouteData { get; set; }

  PermissionSet? PermissionSet { get; set; }

  [Inject]
  NavigationManager? NavigationManager { get; set; }

  [Inject]
  BlazorEngineSecurityService Security { get; set; } = null!;

  Uri? UnauthorizedUri { get; set; }
  Uri? LoginUri { get; set; }

  protected override async Task OnParametersSetAsync()
  {
    UnauthorizedUri ??= NavigationManager!.ToAbsoluteUri(BlazorEngineSettings.Instance.UnauthorizedRoute);
    LoginUri ??= NavigationManager!.ToAbsoluteUri(BlazorEngineSettings.Instance.LoginRoute);

    if ((NavigationManager!.Uri.Equals(UnauthorizedUri.ToString())) || (NavigationManager.Uri.Equals(LoginUri.ToString())))
      return;
    PermissionSet = await Security.GetPermissionSet(RouteData.PageType);

    if (!PermissionSet.Execute)
    {
      NavigationManager?.NavigateTo(BlazorEngineSettings.Instance.UnauthorizedRoute);
    }
    if ((PermissionSet.RequireAuthentication) && string.IsNullOrEmpty(await Security.GetSessionIdentifier()))
    {
      NavigationManager?.NavigateTo(BlazorEngineSettings.Instance.LoginRoute);
    }

  }
}
