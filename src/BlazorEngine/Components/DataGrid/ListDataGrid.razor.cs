using BlazorEngine.Components.Base;
using BlazorEngine.Models;
using BlazorEngine.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorEngine.Components.DataGrid
{
  public partial class ListDataGrid<T> where T : class
  {
    internal T? CurrRec { get; set; }
    FluentMenu? GridActionRef { get; set; }

    [Inject]
    private IKeyCodeService? KeyCodeService { get; set; }
    private int _gridActionsCount;

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
      //RefreshData?.Invoke();
      StateHasChanged();
    }

    protected void HandleDelete(T data)
    {
      OnDiscard?.Invoke(data);
      //RefreshData?.Invoke();
      StateHasChanged();
    }

    protected async void NewItem()
    {
      var item = OnNewItem?.Invoke();
      item ??= Activator.CreateInstance<T>();
      await EditAsync(item);

      StateHasChanged();
    }

    private async void ExportToExcel()
    {
      try
      {
        var dataToExport = Selected.Count > 0 ? Selected.ToList() : Data?.ToList();
        var res = ExcelUtilities.ExportToExcel(dataToExport!, VisibleFields);

        using var streamRef = new DotNetStreamReference(stream: res);
        await JSRuntime!.InvokeVoidAsync("downloadFileFromStream", (Context as BlazorEngineComponentBase)!.ComponentDetached, (Context as BlazorEngineComponentBase)?.Title + ".xlsx", streamRef);

      }
      catch (Exception )
      {
        UIServices.ShowError("Something went wrong while exporting to Excel. Please try again.");
      }
   }

    private void OnKeyDownAsync(FluentKeyCodeEventArgs args)
    {
      if (_multipleSelectEnabled | _shiftModifierEnabled) return;
      if (args.Key == KeyCode.Ctrl)
      {
        _multipleSelectEnabled = true;
      }
      else if (args.Key == KeyCode.Shift)
      {
        _shiftModifierEnabled = true;
      }

    }

    private void OnKeyUp(FluentKeyCodeEventArgs args)
    {
      if (args.Key == KeyCode.Ctrl)
      {
        _multipleSelectEnabled = false;
      }
      else if (args.Key == KeyCode.Shift)
      {
        _shiftModifierEnabled = false;
      }
    }

    private async Task OnSearchBarFocus(FluentKeyCodeEventArgs args)
    {
      if ((args.Key == KeyCode.Function3) || (args.Key == KeyCode.KeyF && args.CtrlKey))
      {
        try
        {
          await SearchBarRef!.Element.FocusAsync();
        }
        catch
        {
          // ignored
        }
      }
    }

    private async Task HandleCellClick(FluentDataGridCell<T> cell)
    {
      if (cell.GridColumn > 1)
      {
        HandleSingleRecSelection(cell.Item);
        cell.ParentReference?.Current.FocusAsync();
        await Task.CompletedTask;
      }
    }

    internal void Refresh()
    {
      StateHasChanged();
    }
  }
}