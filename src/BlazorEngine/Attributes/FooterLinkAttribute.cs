using static Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20;

namespace BlazorEngine.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class FooterLinkAttribute : Attribute
{
  public required string Title { get; set; }
  public required string Route { get; set; }
  public Type Icon { get; set; } = typeof(Balloon);
  public bool OpenNewWindow { get; set; } = true;
}