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
