using BlazorGenerator.Attributes;
using BlazorGenerator.Models;
using BlazorGenerator.Security;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Components.Action
{
    public partial class ActionBar
    {
        [Parameter]
        public IEnumerable<(MethodInfo Method, PageActionAttribute Attribute)> PageActions { get; set; } = [];

    [Parameter]
        public required object Context { get; set; }

    public Dictionary<string, int> ActionGroups { get; set; } = [];

    void PopulateDictionary()
        {
            ActionGroups = [];
            foreach (var item in PageActions)
            {
                if (ActionGroups.TryGetValue(item.Attribute.Group, out int value))
                {
                    ActionGroups[item.Attribute.Group] = ++value;
                }
                else
                {
                    ActionGroups.Add(item.Attribute.Group, 1);
                }
            }
        }
    }
}
