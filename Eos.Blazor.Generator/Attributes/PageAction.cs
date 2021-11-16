using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eos.Blazor.Generator.Attributes
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  public class PageActionAttribute : Attribute
  {
    public PageActionAttribute(string caption)
    {
      Caption = caption;
    }
    public PageActionAttribute(string caption, string icon)
    {
      Caption = caption;
      Icon = icon;
    }

    public PageActionAttribute()
    {
    }

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    public string? Caption { get; private set; }
    public string? Icon { get; private set; } = "oi oi-play-circle";
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
  }
}
