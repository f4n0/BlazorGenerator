using static Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20;

namespace BlazorEngine.Attributes
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class AddToMenuAttribute : Attribute
  {
    public required string Title { get; set; }
    public required string Route { get; set; }
    public Type Icon { get; set; } = typeof(Balloon);
    public string Group { get; set; } = "Default";
    public int OrderSequence { get; set; } = 999;
  }
}
