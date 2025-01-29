using System.Collections.ObjectModel;
using BlazorEngine.Components.Base;
using BlazorEngine.Models;

namespace BlazorEngine.Layouts
{
  public partial class ListPage<T> : BlazorEngineComponentBase where T : class
  {
    public IEnumerable<T>? Content { get; set; }
    public required List<VisibleField<T>> VisibleFields { get; set; } = new();
    public virtual Type? EditFormType { get; set; }
    public virtual bool ShowExportToExcel { get; set; } = true;

    public ObservableCollection<T> Selected { get; set; } = [];
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
    
    async void RefreshData() => await LoadData();
    
    
    public override void InternalDispose()
    {
      GC.SuppressFinalize(this);
      base.InternalDispose();
    }

    public override ValueTask InternalDisposeAsync()
    {
      GC.SuppressFinalize(this);
      return base.InternalDisposeAsync();
    }
  }
}
