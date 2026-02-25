using static Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20;

namespace BlazorEngine.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class PageActionAttribute : Attribute
{
  public required string Caption { get; set; }
  public string Group { get; set; } = "Default";
  public Type Icon { get; set; } = typeof(Run);
}