using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
