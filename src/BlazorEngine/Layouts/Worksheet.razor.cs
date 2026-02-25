using BlazorEngine.Components.Base;
using BlazorEngine.Components.Card;
using BlazorEngine.Components.DataGrid;
using BlazorEngine.Models;
using System.Collections.ObjectModel;

namespace BlazorEngine.Layouts
{
  public partial class Worksheet<TData, TList> : BlazorEngineComponentBase where TList : class
  {
    public virtual int GridSize => 6;
    public TData? Content { get; set; }
    public List<VisibleField<TData>> VisibleFields { get; set; } = [];

    public IEnumerable<TList>? ListContent { get; set; }
    public List<VisibleField<TList>> ListVisibleFields { get; set; } = [];
    public virtual Type? ListEditFormType { get; set; }
    public virtual bool ShowExportToExcel { get; set; } = true;
    public ObservableCollection<TList> ListSelected { get; set; } = [];
    private CardFields<TData>? Card { get; set; }
    private ListDataGrid<TList>? List { get; set; }

    public virtual bool UseVirtualization { get; set; } = false;

    public virtual void OnSave(TList entity)
    {
    }

    public virtual void OnDelete(TList entity)
    {
    }

    public virtual void OnSave(TData entity)
    {
    }

    public virtual void OnDelete(TData entity)
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


    public void RefreshCard()
    {
      if (Card != null)
      {
        Card.Data = Content;
        Card.VisibleFields = VisibleFields;
        Card.Refresh();
      }
    }
    public void RefreshList()
    {
      if (List != null)
      {
        List.Data = ListContent?.AsQueryable();
        List.VisibleFields = ListVisibleFields;
        List.Refresh();
      }
    }
  }
}