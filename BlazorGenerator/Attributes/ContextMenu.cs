using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator.Attributes
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class ContextMenuAttribute : Attribute
  {
    public required string Caption { get; set; }
    public object Icon { get; set; } = typeof(Icons.Regular.Size20.Balloon);
  }
}
