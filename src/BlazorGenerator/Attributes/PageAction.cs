using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator.Attributes
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class PageActionAttribute : Attribute
  {
    public required string Caption { get; set; }
    public string Group { get; set; } = "Default";
    public Type Icon { get; set; } = typeof(Icons.Regular.Size20.Run);
  }
}
