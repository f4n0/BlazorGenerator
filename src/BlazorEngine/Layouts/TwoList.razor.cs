using BlazorEngine.Components.Base;
using BlazorEngine.Models;
using System.Collections.ObjectModel;

namespace BlazorEngine.Layouts
{
  public partial class TwoList<TFirstList, TSecondList> : BlazorEngineComponentBase
    where TFirstList : class
    where TSecondList : class
  {
    public IEnumerable<TFirstList>? FirstListContent { get; set; }
    public required List<VisibleField<TFirstList>> FirstListVisibleFields { get; set; }
    public virtual Type? FirstListEditFormType { get; set; }
    public ObservableCollection<TFirstList> FirstListSelected { get; set; } = [];
    public virtual bool ShowExportToExcelFirstList { get; set; } = true;

    public IEnumerable<TSecondList>? SecondListContent { get; set; }
    public required List<VisibleField<TSecondList>> SecondListVisibleFields { get; set; }
    public virtual Type? SecondListEditFormType { get; set; }
    public ObservableCollection<TSecondList> SecondListSelected { get; set; } = [];
    public virtual bool ShowExportToExcelSecondList { get; set; } = true;



    public virtual void OnSave(TFirstList entity)
    {
    }

    public virtual void OnDelete(TFirstList entity)
    {
    }

    public virtual void OnSave(TSecondList entity)
    {
    }

    public virtual void OnDelete(TSecondList entity)
    {
    }


    private PermissionSet permissionSet;

    protected override async Task OnInitializedAsync()
    {
      permissionSet = await Security.GetPermissionSet(this.GetType());
      await base.OnInitializedAsync();
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
}
