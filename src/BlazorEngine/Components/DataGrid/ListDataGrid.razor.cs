using System.Runtime.CompilerServices;
using BlazorEngine.Components.Base;
using BlazorEngine.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;
using Microsoft.JSInterop;

namespace BlazorEngine.Components.DataGrid;

public partial class ListDataGrid<T> where T : class
{
  private static readonly Icon MoreVerticalIcon = new Size16.MoreVertical();
  private static readonly Icon EditIcon = new Size16.Edit();
  private static readonly Icon DeleteIcon = new Size16.Delete();
  private static readonly Icon AddIcon = new Size16.Add();
  private static readonly Icon ExportIcon = new Size20.DocumentTableArrowRight();

  private readonly ConditionalWeakTable<T, string> _rowIds = new();
  private int _gridActionsCount;

  internal T? CurrRec { get; set; }
  private FluentMenu? GridActionRef { get; set; }

  [Inject] private IKeyCodeService? KeyCodeService { get; set; }

  private string GetRowId(T item)
  {
    return _rowIds.GetValue(item, _ => Identifier.NewId());
  }

  protected override void OnParametersSet()
  {
    _gridActionsCount = GridActions?.Count() ?? 0;
    base.OnParametersSet();
  }

  protected override async Task OnInitializedAsync()
  {
    //for global F3, otherwise it won't work as soon the page load
    KeyCodeService?.RegisterListener(OnSearchBarFocus);
    await base.OnInitializedAsync();
  }

  private async Task EditAsync(T context)
  {
    T? res;
    if (EditFormType != null)
    {
      res = await UIServices!.OpenPanel(EditFormType, context);
      HandleSave(res);
    }
  }

  protected void HandleSave(T? data)
  {
    OnSave?.Invoke(data);
    InvokeAsync(() => StateHasChanged());
  }

  protected void HandleDelete(T data)
  {
    OnDiscard?.Invoke(data);
    InvokeAsync(() => StateHasChanged());
  }

  protected async Task NewItem()
  {
    var item = OnNewItem?.Invoke();
    item ??= Activator.CreateInstance<T>();
    await EditAsync(item);

    await InvokeAsync(() => StateHasChanged());
  }

  private async Task ExportToExcel()
  {
    try
    {
      var dataToExport = Selected.Count > 0 ? Selected.ToList() : Data?.ToList();
      var res = ExcelUtilities.ExportToExcel(dataToExport!, VisibleFields);

      using var streamRef = new DotNetStreamReference(res);
      await JSRuntime!.InvokeVoidAsync("downloadFileFromStream",
        (Context as BlazorEngineComponentBase)!.ComponentDetached,
        (Context as BlazorEngineComponentBase)?.Title + ".xlsx", streamRef);
    }
    catch (Exception)
    {
      UIServices.ShowError("Something went wrong while exporting to Excel. Please try again.");
    }
  }

  private void OnKeyDownAsync(FluentKeyCodeEventArgs args)
  {
    if (_ctrlPressed) return;
    if (args.Key == KeyCode.Ctrl) _ctrlPressed = true;
  }

  private void OnKeyUpAsync(FluentKeyCodeEventArgs args)
  {
    if (args.Key == KeyCode.Ctrl) _ctrlPressed = false;
  }

  private async Task OnSearchBarFocus(FluentKeyCodeEventArgs args)
  {
    if (args.Key == KeyCode.Function3 || (args.Key == KeyCode.KeyF && args.CtrlKey))
      try
      {
        await SearchBarRef!.Element.FocusAsync();
      }
      catch
      {
        // ignored
      }
  }

  private async Task HandleRowDoubleClick(FluentDataGridRow<T> row)
  {
    if (row.Item != null && PermissionSet?.Modify == true) await EditAsync(row.Item);
  }

  internal void Refresh()
  {
    InvokeAsync(() => StateHasChanged());
  }
}