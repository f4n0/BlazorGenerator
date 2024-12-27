using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;


namespace BlazorGenerator.Security;
public partial class BlazorGeneratorSecurity : ComponentBase
{
  [Parameter]
  public required RenderFragment ChildContent { get; set; }

  [Parameter]
  public required RouteData RouteData { get; set; }

  PermissionSet? permissionSet { get; set; }

  [Inject]
  NavigationManager? NavigationManager { get; set; }

  [Inject]
  BlazorGeneratorSecurityService Security { get; set; } = null!;

  Uri? UnauthorizedUri { get; set; }
  Uri? LoginUri { get; set; }

  protected override async Task OnParametersSetAsync()
  {
    UnauthorizedUri ??= NavigationManager!.ToAbsoluteUri(BlazorGeneratorSettings.Instance.UnauthorizedRoute);
    LoginUri ??= NavigationManager!.ToAbsoluteUri(BlazorGeneratorSettings.Instance.LoginRoute);

    if ((NavigationManager!.Uri.Equals(UnauthorizedUri.ToString())) || (NavigationManager.Uri.Equals(LoginUri.ToString())))
      return;
    permissionSet = await Security.GetPermissionSet(RouteData.PageType);

    if (!permissionSet.Execute)
    {
      NavigationManager?.NavigateTo(BlazorGeneratorSettings.Instance.UnauthorizedRoute);
    }
    if ((permissionSet.RequireAuthentication) && string.IsNullOrEmpty(await Security.GetSessionIdentifier()))
    {
      NavigationManager?.NavigateTo(BlazorGeneratorSettings.Instance.LoginRoute);
    }
        
  }
}
