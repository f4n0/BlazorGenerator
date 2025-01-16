namespace BlazorEngine.Models;

public class VisibleFieldSetterArgs<T> : VisibleFieldBaseArgs<T>
{
  public object? Value { get; set; }
}