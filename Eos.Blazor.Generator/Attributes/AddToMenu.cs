using Blazorise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eos.Blazor.Generator.Attributes
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class AddToMenuAttribute : Attribute
  {
    public AddToMenuAttribute(string title, string route, IconName icon = IconName.Add)
    {
      Title = title;
      Route = route;
      Icon = icon;
    }

    public string Title { get; private set; }
    public string Route { get; private set; }
    public IconName Icon { get; private set; }
  }
}
