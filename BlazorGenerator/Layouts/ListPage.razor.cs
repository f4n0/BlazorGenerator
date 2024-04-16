using BlazorGenerator.Components.Base;
using BlazorGenerator.Models;

namespace BlazorGenerator.Layouts
{
  public partial class ListPage<T> : BlazorgenComponentBase where T : class
  {
    public IEnumerable<T>? Content { get; set; }
    public required List<VisibleField<T>> VisibleFields { get; set; }
    public virtual Type? EditFormType { get; set; }

    public List<T> Selected { get; set; } = [];

    public virtual void OnSave(T entity)
    {
    }
    public virtual T NewItem()
    {
      return Activator.CreateInstance<T>();
    }


    public virtual void OnDelete(T entity)
    {
    }
  }
}
