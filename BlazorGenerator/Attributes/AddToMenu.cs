using Microsoft.FluentUI.AspNetCore.Components;
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

    public required string Title { get; set; }
    public required string Route { get; set; }
    public Type Icon { get; set; } = typeof(Icons.Regular.Size20.Balloon);
    public string Group { get; set; } = "Default";
    public int OrderSequence { get; set; }
  }
}
