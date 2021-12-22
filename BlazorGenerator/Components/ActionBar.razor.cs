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
  }
}
