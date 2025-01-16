using static Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20;

namespace BlazorEngine.Attributes
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class GridActionAttribute : Attribute
  {
    public required string Caption { get; set; }
    public Type GridIcon { get; set; } = typeof(Run);
  }
}
