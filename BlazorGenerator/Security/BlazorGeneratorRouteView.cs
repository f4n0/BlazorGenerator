using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Security
{
  public class BlazorGeneratorRouteView : RouteView
  {
    [Inject]
    NavigationManager NavigationManager { get; set; }

    [Inject]
    BlazorGeneratorSecurity Security { get; set; }

    protected override void Render(RenderTreeBuilder builder)
    {
      if (Security.GetPermissionSet(RouteData.PageType).Execute)
      {
        base.Render(builder);
      }
      else
      {
        NavigationManager.NavigateTo("/Unauthorized");
      }
    }
  }
}
