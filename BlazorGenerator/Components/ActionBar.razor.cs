using BlazorGenerator.Attributes;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Components
{
  partial class ActionBar
  {
    [Parameter]
    public IEnumerable<(MethodInfo Method, PageActionAttribute Attribute)> PageActions { get; set; }

    [Parameter]
    public object Context { get; set; }

    [Inject]
    public BlazorGenOptions options { get; set; }

    private Blazorise.TextColor TextColor => options.UseDarkTheme ? Blazorise.TextColor.White : Blazorise.TextColor.Dark;

    private Blazorise.Background BackgroundColor => options.UseDarkTheme ? Blazorise.Background.Secondary : Blazorise.Background.Light;

    private Blazorise.IFluentBorder Borders => options.UseDarkTheme ? Blazorise.Border.Is1.Rounded.White : Blazorise.Border.Is1.Rounded.Dark;

    public Dictionary<string, int> ActionGroups { get; set; }

    void PopulateDictionary()
    {
      ActionGroups = new Dictionary<string, int>();
      foreach (var item in PageActions)
      {
        if (ActionGroups.ContainsKey(item.Attribute.Group))
        {          
          ActionGroups[item.Attribute.Group]++;
        }
        else
        {
          ActionGroups.Add(item.Attribute.Group, 1);
        }
      }
    }
  }
}
