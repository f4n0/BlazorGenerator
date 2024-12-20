using Microsoft.FluentUI.AspNetCore.Components;
using static Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20;

namespace BlazorGenerator.Attributes
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class GridActionAttribute : Attribute
  {
    public required string Caption { get; set; }
    public Type GridIcon { get; set; } = typeof(Run);
  }
}
