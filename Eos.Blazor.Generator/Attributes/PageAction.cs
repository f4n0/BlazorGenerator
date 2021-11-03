using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eos.Blazor.Generator.Attributes
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  public class PageAction : Attribute
  {
    public PageAction(string caption)
    {
      Caption = caption;
    }
    public PageAction(string caption, string icon)
    {
      Caption = caption;
      Icon = icon;
    }

    public PageAction()
    {
    }

    public string? Caption { get; private set; }
    public string? Icon { get; private set; } = "oi oi-play-circle";
  }
}
