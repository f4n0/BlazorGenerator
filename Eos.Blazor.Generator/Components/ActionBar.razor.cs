using Eos.Blazor.Generator.Attributes;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Eos.Blazor.Generator.Components
{
  partial class ActionBar
  {
    [Parameter]
    public IEnumerable<(MethodInfo Method, PageAction Attribute)> PageActions { get; set; }

    [Parameter]
    public object Context { get; set; }
  }
}
