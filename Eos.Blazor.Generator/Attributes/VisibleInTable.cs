using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eos.Blazor.Generator.Attributes
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
  public class Visible : Attribute
  {
    public Visible(string caption)
    {
      Caption = caption;
    }
    public Visible()
    {
    }

    public string? Caption { get; private set; }
  }
}
