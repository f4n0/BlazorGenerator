using static Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20;

namespace BlazorGenerator.Attributes
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class ContextMenuAttribute : Attribute
  {
    public required string Caption { get; set; }
    public object Icon { get; set; } = typeof(Balloon);
  }
}
