using Microsoft.AspNetCore.Components;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Eos.BlazorGenerator.Attributes
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class AddToMenu : Attribute
  {
    public AddToMenu(string title, string route)
    {
      Title = title;
      Route = route;
    }

    public string Title { get; private set; }
    public string Route { get; private set; }    
  }

}
