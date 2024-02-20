using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator.Attributes
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class PageActionAttribute : Attribute
  {
    public PageActionAttribute()
    {
    }

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    public string Caption { get; set; }
    public string Group { get; set; } = "Default";
    public Type Icon { get; set; } = typeof(Icons.Regular.Size20.Run);

#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
  }
}
