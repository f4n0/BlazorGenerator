using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eos.Blazor.Generator.Attributes
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class AddToMenu : Attribute
  {
    public AddToMenu(string title, string route, string icon = "oi oi-list-rich")
    {
      Title = title;
      Route = route;
      Icon = icon;
    }

    public string Title { get; private set; }
    public string Route { get; private set; }
    public string Icon { get; private set; }
  }
}
