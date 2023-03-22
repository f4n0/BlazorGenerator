using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using System.Reflection;
using System.ComponentModel;
using BlazorGenerator.Attributes;
using BlazorGenerator.Utils;
using BlazorGenerator.Models;
using BlazorGenerator.Services;
using Blazorise;
using Blazorise.DataGrid;
using Blazorise.Components;
using BlazorGenerator.Dialogs;
using BlazorGenerator.Pages;
using BlazorGenerator.Components;
using BlazorGenerator.Infrastructure;
using BlazorGenerator.Security;

namespace BlazorGenerator.Components
{
  public partial class NavMenu
  {
    Dictionary<string, int> MenuGroups { get; set; }

    List<AddToMenuAttribute> menus { get; set; } = new List<AddToMenuAttribute>();

    Bar Menu { get; set; }

    void PopulateDictionary()
    {
      var allMenu = Utils.AttributesUtils.GetModelsWithAttribute<AddToMenuAttribute>();
      MenuGroups = new Dictionary<string, int>();
      foreach (var item in allMenu)
      {
        if (security.GetPermissionSet(item.Type).Execute)
        {
          if(!menus.Contains(item.Attribute))
            menus.Add(item.Attribute);
          if (MenuGroups.ContainsKey(item.Attribute.Group))
          {
            MenuGroups[item.Attribute.Group]++;
          }
          else
          {
            MenuGroups.Add(item.Attribute.Group, 1);
          }
        }
      }
    }
  }
}