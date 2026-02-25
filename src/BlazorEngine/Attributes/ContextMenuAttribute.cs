using static Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20;

namespace BlazorEngine.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class ContextMenuAttribute : Attribute
{
  public required string Caption { get; set; }
  public object Icon { get; set; } = typeof(Balloon);
}