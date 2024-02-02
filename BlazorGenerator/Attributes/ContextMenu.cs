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

    public string? Caption { get; set; }
    public object? Icon { get; set; }
  }
}
