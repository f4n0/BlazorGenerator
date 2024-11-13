using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;


namespace BlazorGenerator.Security;
public partial class BlazorGeneratorSecurity
{
  [Parameter]
  public required RenderFragment ChildContent { get; set; }

  [Parameter]
  public required RouteData RouteData { get; set; }

  PermissionSet permissionSet { get; set; }

  [Inject]
  NavigationManager? NavigationManager { get; set; }

  [Inject]
  BlazorGeneratorSecurityService Security { get; set; } = null!;

  protected override async Task OnInitializedAsync()  {

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
