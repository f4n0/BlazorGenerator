using Blazorise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Attributes
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class ContextMenuAttribute : Attribute
  {

    public ContextMenuAttribute()
    {
    }

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    public string? Caption { get; set; }
    public object Icon { get; set; } = IconName.Add;
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
  }
}
