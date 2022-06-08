using Blazorise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Attributes
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class AddToMenuAttribute : Attribute
  {
    public AddToMenuAttribute()
    {
    }

    public string Title { get; set; }
    public string Route { get; set; }
    public IconName Icon { get; set; } = Blazorise.IconName.Add;
    public string Group { get; set; } = "Default";
    public int OrderSequence { get; set; }
  }
}
