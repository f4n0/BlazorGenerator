using System.Reflection;
using System.Runtime.CompilerServices;
using BlazorEngine.Components.Base;
using BlazorEngine.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;
using Microsoft.JSInterop;

namespace BlazorEngine.Components.DataGrid;

public partial class ListDataGrid<T> : IDisposable, IAsyncDisposable where T : class
{
  private static readonly KeyCode[] SearchKeyCodes =
  {
    KeyCode.Function3,
    KeyCode.KeyF,
    KeyCode.Ctrl,
    KeyCode.Shift
  };

  private static readonly Icon MoreVerticalIcon = new Size16.MoreVertical();
  private static readonly Icon EditIcon = new Size16.Edit();
  private static readonly Icon DeleteIcon = new Size16.Delete();
  private static readonly Icon AddIcon = new Size16.Add();
  private static readonly Icon ExportIcon = new Size20.DocumentTableArrowRight();

  private readonly ConditionalWeakTable<T, string> _rowIds = new();
  private int _gridActionsCount;
  private bool _searchShortcutRegistered;

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
    InvalidateFilterCache();
    InvokeAsync(() =>
    {
      RefreshSelectionSnapshot();
      StateHasChanged();
    });
  }

  protected void HandleDelete(T data)
  {
    OnDiscard?.Invoke(data);
    InvalidateFilterCache();
    InvokeAsync(() =>
    {
      RefreshSelectionSnapshot();
      StateHasChanged();
    });
  }

  protected async Task NewItem()
  {
    var item = OnNewItem?.Invoke();
    item ??= Activator.CreateInstance<T>();
    await EditAsync(item);

    InvalidateFilterCache();
    await InvokeAsync(() =>
    {
      RefreshSelectionSnapshot();
      StateHasChanged();
    });
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
      await UIServices!.ShowErrorAsync("Something went wrong while exporting to Excel. Please try again.");
    }
  }

  private async Task OnKeyDownAsync(FluentKeyCodeEventArgs args)
  {
    if (args.Key == KeyCode.Ctrl)
    {
      _ctrlPressed = true;
      return;
    }

    if (args.Key == KeyCode.Shift)
    {
      _shiftPressed = true;
      return;
    }

    if (args.Key == KeyCode.Function3 || (args.Key == KeyCode.KeyF && (args.CtrlKey || _ctrlPressed)))
      await FocusSearchAsync();
  }

  private void OnKeyUpAsync(FluentKeyCodeEventArgs args)
  {
    if (args.Key == KeyCode.Ctrl) _ctrlPressed = false;
    if (args.Key == KeyCode.Shift) _shiftPressed = false;
  }


  private async Task FocusSearchAsync()
  {
    if (SearchBarRef?.Element is not { } searchElement)
      return;

    try
    {
      await searchElement.FocusAsync();
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

  private Task InvokeGridAction(MethodInfo method, T rowData)
  {
    if (GridActionRef != null)
      GridActionRef.Open = false;

    return ReflectionUtilites.InvokeAction(method, Context, new object[] { rowData });
  }

  internal void Refresh()
  {
    InvalidateFilterCache();
    InvokeAsync(() =>
    {
      RefreshSelectionSnapshot();
      StateHasChanged();
    });
  }

  public void Dispose()
  {
    GC.SuppressFinalize(this);
  }

  public ValueTask DisposeAsync()
  {
    GC.SuppressFinalize(this);
    return ValueTask.CompletedTask;
  }

}