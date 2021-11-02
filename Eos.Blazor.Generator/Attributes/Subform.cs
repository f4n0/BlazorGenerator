using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eos.Blazor.Generator.Attributes
{
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
  public class Subform : Attribute
  {
    public Subform(string caption)
    {
      Caption = caption;
    }

    public Subform()
    {
    }

    public string? Caption { get; private set; }
  }
}
