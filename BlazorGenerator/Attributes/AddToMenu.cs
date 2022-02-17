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
    public AddToMenuAttribute(string title, string route, IconName icon = IconName.Add, string group = "default")
    {
      Title = title;
      Route = route;
      Icon = icon;
      Group = group;
    }

    public string Title { get; private set; }
    public string Route { get; private set; }
    public IconName Icon { get; private set; }
    public string Group { get; private set; }
  }
}
