using System.Collections.ObjectModel;
using BlazorEngine.Actions;
using BlazorEngine.Fields;
using BlazorEngine.Components.Base;
using BlazorEngine.Components.Card;
using BlazorEngine.Components.DataGrid;
using BlazorEngine.Models;

namespace BlazorEngine.Layouts;

public partial class Worksheet<TData, TList> : BlazorEngineComponentBase where TList : class
{
  protected override bool UsesPageLifecycle => true;
  protected virtual IReadOnlyList<GridActionDefinition<TList>> CreateGridActions() => [];
  protected virtual Task ConfigureFields(FieldListBuilder<TData> fields) => Task.CompletedTask;
  protected virtual Task ConfigureListFields(FieldListBuilder<TList> fields) => Task.CompletedTask;

  protected override async Task LoadVisibleFields()
  {
    var fields = new FieldListBuilder<TData>();
    await ConfigureFields(fields);
    VisibleFields = fields.Build();
    var listFields = new FieldListBuilder<TList>();
    await ConfigureListFields(listFields);
    ListVisibleFields = listFields.Build();
  }

  private PermissionSet permissionSet = new();
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

  protected override async Task OnInitializedAsync()
  {
    permissionSet = await Security.GetPermissionSet(GetType());
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
      List.Data = ListContent;
      List.VisibleFields = ListVisibleFields;
      List.Refresh();
    }
  }
}
