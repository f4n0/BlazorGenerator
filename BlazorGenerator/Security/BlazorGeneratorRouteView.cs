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

      if ((await Security?.GetPermissionSet(RouteData.PageType)).Execute)
      {
        // do nothing
      }
      else
      {
        NavigationManager?.NavigateTo("/Unauthorized");
      }
      await Task.CompletedTask;
    }

  }
}
