namespace BlazorGenerator.Attributes
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class ContextMenuAttribute : Attribute
  {
    public ContextMenuAttribute()
    {
    }

    public string Caption { get; set; }
    public object Icon { get; set; }
  }
}
