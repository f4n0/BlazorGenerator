using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Security
{
  public class BlazorgenRouteView : RouteView
  {
    [Inject]
    NavigationManager NavigationManager { get; set; }
    [Inject]
    ISecurity Security { get; set; } = null;

    protected override void Render(RenderTreeBuilder builder)
    {
      if (Security?.HasPermission(RouteData.PageType) ?? true)
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
