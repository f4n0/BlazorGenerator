using Newtonsoft.Json;

namespace BlazorGenerator.Models;

public class VisibleFieldBaseArgs<T>
{
  public required T Data { get; set; }
  public required VisibleField<T> Field { get; set; }
}