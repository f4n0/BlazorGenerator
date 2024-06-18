using BlazorGenerator.Components.Base;
using BlazorGenerator.Models;
using BlazorGenerator.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorGenerator.Components.DataGrid
{
  public partial class ListDataGrid<T> where T : class
  {
    internal T? CurrRec { get; set; }

    protected override async Task OnInitializedAsync()
    {      
      await base.OnInitializedAsync();
    }

    private async Task EditAsync(T context)
    {
      T? res;
      if (EditFormType != null)
      {
        res = await UIServices!.OpenPanel<T>(EditFormType, context);
      }
      else
      {
        var typeDelegate = RoslynUtilities.CreateAndInstatiateClass(VisibleFields, "edit");
        var type = (Type)typeDelegate.Invoke().Result;
        res = await UIServices!.OpenPanel<T>(type, context);
        GC.Collect();
      }
      HandleSave(res!);
    }

    protected void HandleSave(T Data)
    {
      OnSave?.Invoke(Data);
      //RefreshData?.Invoke();
      StateHasChanged();
    }

    protected void HandleDiscard(T Data)
    {
      OnDiscard?.Invoke(Data);
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

    string GetCssGridTemplate(int GridActions, PermissionSet? permissionSet)
    {
      const string select = "50px ";
      string actions = string.Empty;
      if (GridActions > 0)
        actions = "30px ";

      var spacing = 80 / VisibleFields.Count;
      string cols = "repeat(auto-fill," + spacing + "%) ";
      string rowActions = string.Empty;
      if ((permissionSet?.Modify ?? false) || (permissionSet?.Delete ?? false))
        rowActions = "100px";

      return select + actions + cols + rowActions;
    }

    private async void ExportToExcel()
    {
      var DataToExport = Selected.Count > 0 ? Selected : Data?.ToList();
      var res = ExcelUtilities.ExportToExcel(DataToExport!, VisibleFields);

      using var streamRef = new DotNetStreamReference(stream: res);
      await JSRuntime!.InvokeVoidAsync("downloadFileFromStream", (Context as BlazorGeneratorComponentBase)?.Title + ".xlsx", streamRef);
    }

    private async Task OnKeyDownAsync(FluentKeyCodeEventArgs args)
    {
      if ((args.Key == KeyCode.Function3) || (args.Key == KeyCode.KeyF && args.CtrlKey))
      {
        try
        {
          await SearchBarRef!.Element.FocusAsync();
        }
        catch
        {
        }
      }
      else if (args.Key == KeyCode.Ctrl)
      {
        MultipleSelectEnabled = true;
      }
    }

    private void OnKeyUp(FluentKeyCodeEventArgs args)
    {
      if (args.Key == KeyCode.Ctrl)
      {
        MultipleSelectEnabled = false;
      }
    }
  }
}
