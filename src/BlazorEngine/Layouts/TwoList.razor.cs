using System.Collections.ObjectModel;
using BlazorEngine.Actions;
using BlazorEngine.Fields;
using BlazorEngine.Components.Base;
using BlazorEngine.Models;

namespace BlazorEngine.Layouts;

public partial class TwoList<TFirstList, TSecondList> : BlazorEngineComponentBase
  where TFirstList : class
  where TSecondList : class
{
  protected override bool UsesPageLifecycle => true;
  protected virtual IReadOnlyList<GridActionDefinition<TFirstList>> CreateFirstListGridActions() => [];
  protected virtual IReadOnlyList<GridActionDefinition<TSecondList>> CreateSecondListGridActions() => [];
  protected virtual Task ConfigureFirstListFields(FieldListBuilder<TFirstList> fields) => Task.CompletedTask;
  protected virtual Task ConfigureSecondListFields(FieldListBuilder<TSecondList> fields) => Task.CompletedTask;

  protected override async Task LoadVisibleFields()
  {
    var firstFields = new FieldListBuilder<TFirstList>();
    await ConfigureFirstListFields(firstFields);
    FirstListVisibleFields = firstFields.Build();
    var secondFields = new FieldListBuilder<TSecondList>();
    await ConfigureSecondListFields(secondFields);
    SecondListVisibleFields = secondFields.Build();
  }

  private PermissionSet permissionSet = new();
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
}
