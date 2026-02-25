using System.Collections.ObjectModel;
using BlazorEngine.Components.Base;
using BlazorEngine.Models;

namespace BlazorEngine.Layouts;

public partial class ListPage<T> : BlazorEngineComponentBase where T : class
{
  private PermissionSet permissionSet;
  public IEnumerable<T>? Content { get; set; }
  public required List<VisibleField<T>> VisibleFields { get; set; } = new();
  public virtual Type? EditFormType { get; set; }
  public virtual bool ShowExportToExcel { get; set; } = true;

  public virtual bool UseVirtualization { get; set; } = false;

  public ObservableCollection<T> Selected { get; set; } = [];

  public virtual void OnSave(T? entity)
  {
  }

  public virtual T NewItem()
  {
    return Activator.CreateInstance<T>();
  }

  public virtual void OnDelete(T entity)
  {
  }

  protected override async Task OnInitializedAsync()
  {
    permissionSet = await Security.GetPermissionSet(GetType());
    await base.OnInitializedAsync();
  }


  private async Task RefreshData()
  {
    await LoadData();
  }


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