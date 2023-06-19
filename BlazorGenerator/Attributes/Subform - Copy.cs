using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Attributes
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  public class MenuFooter : Attribute
  {
    public MenuFooter()
    {
    }
  }
}
