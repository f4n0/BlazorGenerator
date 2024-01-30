using BlazorGenerator.Security;
using BlazorGenerator.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazorGenerator.Models;

namespace BlazorGenerator.Components.Base
{
  public partial class BlazorgenComponentBase : ComponentBase, IDisposable
  {


    public virtual string Title => this.GetType().Name;

    public virtual bool ShowButtons { get; set; } = true;
    public virtual bool ShowActions { get; set; } = true;


    public void Dispose()
    {
    }


    internal string GetCssGridTemplate(int GridActions, PermissionSet permissionSet)
    {
      string select = "50px ";
      string actions = string.Empty;
      if (GridActions > 0)
        actions = "50px ";

      string cols = "repeat(auto-fill,14%) ";
      string rowActions = string.Empty;
      if (permissionSet.Modify || permissionSet.Delete)
        rowActions = "150px";

      return select+actions+cols+rowActions;

    }
  }
}
