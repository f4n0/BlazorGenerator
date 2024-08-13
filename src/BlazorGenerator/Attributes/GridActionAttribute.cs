using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator.Attributes
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class GridActionAttribute : Attribute
  {
    public required string Caption { get; set; }
    public Type GridIcon { get; set; } = typeof(Icons.Regular.Size20.Run);
  }
}
