using Microsoft.AspNetCore.Components;

namespace BlazorGenerator.Security
{
  public class BlazorGeneratorRouteView : RouteView, IHandleAfterRender
  {
    [Inject]
    NavigationManager? NavigationManager { get; set; }

    [Inject]
    BlazorGeneratorSecurity? Security { get; set; }

    public async Task OnAfterRenderAsync()
    {
      var permissionSet = await Security?.GetPermissionSet(RouteData.PageType);

      if (!permissionSet.Execute)
      {
        NavigationManager?.NavigateTo(BlazorGeneratorSettings.Instance.UnauthorizedRoute);
      }
      if ((permissionSet.RequireAuthentication) && string.IsNullOrEmpty(await Security.GetSessionIdentifier()))
      {
        NavigationManager?.NavigateTo(BlazorGeneratorSettings.Instance.UnauthorizedRoute);
      }
      await Task.CompletedTask;
    }

  }
}
