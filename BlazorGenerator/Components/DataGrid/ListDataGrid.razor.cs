using BlazorGenerator.Attributes;
using BlazorGenerator.Components.Base;
using BlazorGenerator.Models;
using BlazorGenerator.Services;
using BlazorGenerator.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Reflection;

namespace BlazorGenerator.Components.DataGrid
{
  public partial class ListDataGrid<T> where T : class
  {
    [Inject]
    public UIServices? UIServices { get; set; }

    [Inject]
    IJSRuntime? JSRuntime { get; set; }

    [Parameter]
    public required object Context { get; set; }

    [Parameter]
    public required List<VisibleField<T>> VisibleFields { get; set; }

    [Parameter]
    public IQueryable<T>? Data { get; set; }

    IQueryable<T>? FilteredData => FilterData();

    [Parameter]
    public bool ShowButtons { get; set; }

    [Parameter]
    public PermissionSet? PermissionSet { get; set; }

    [Parameter]
    public IEnumerable<(MethodInfo Method, GridActionAttribute Attribute)>? GridActions { get; set; } = [];

    [Parameter]
    public List<T> Selected { get; set; } = [];

    [Parameter]
    public EventCallback<List<T>> SelectedChanged { get; set; }

    [Parameter]
    public Action? RefreshData { get; set; }

    internal T? CurrRec { get; set; }

    [Parameter]
    public virtual Type? EditFormType { get; set; }

    string SearchValue = string.Empty;

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

    internal Func<T, string> SelectedRowClass => (data) => Selected.Contains(data) ? "rowselected" : "";

    private void HandleRecSelection(bool selected, T Rec)
    {
      if (selected)
      {
        Selected.Add(Rec);
      }
      else
      {
        Selected.Remove(Rec);
      }
    }
    private void HandleSingleRecSelection(T? Rec)
    {
      Selected.Clear();
      if (Rec != null)
      {
        Selected.Add(Rec);
      }
    }
    private bool? AllRecSelected
    {
      get
      {
        return Selected.Count == Data?.Count()
            ? true
            : Selected.Count == 0
                ? false
                : null;
      }
      set
      {
        if (value is true)
        {
          Selected.Clear();
          if (FilteredData != null)
          {
            Selected.AddRange(FilteredData);
          }
        }
        else if (value is false)
        {
          Selected.Clear();
        }
      }
    }

    [Parameter]
    public Action<T>? OnSave { get; set; }

    protected void HandleSave(T Data)
    {
      OnSave?.Invoke(Data);
      //RefreshData?.Invoke();
      StateHasChanged();
    }

    [Parameter]
    public Action<T>? OnDiscard { get; set; }

    protected void HandleDiscard(T Data)
    {
      OnDiscard?.Invoke(Data);
      //RefreshData?.Invoke();
      StateHasChanged();
    }

    [Parameter]
    public Func<T>? OnNewItem { get; set; }

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

    Dictionary<string, string> FieldFilters { get; set; } = [];

    private string GetFilterValue(string fieldName)
    {
      if (FieldFilters.TryGetValue(fieldName, out var res))
      {
        return res;
      }
      else
      {
        FieldFilters.Add(fieldName, string.Empty);
        return string.Empty;
      }
    }

    private void HandleFilter(ChangeEventArgs e, VisibleField<T> field)
    {
      if (e.Value is string value)
      {
        FieldFilters[field.Name] = value;
      }
    }

    IQueryable<T>? FilterData()
    {
      if (Data is null) return null;
      var set = Data;
      //first global search
      if (!string.IsNullOrWhiteSpace(SearchValue))
      {
        set = set.AsEnumerable().Where(r =>
        {
          foreach (var field in VisibleFields)
          {
            var CellValue = field.Getter(r);
            var cellStringValue = CellValue == null ? string.Empty : CellValue.ToString();
            if (cellStringValue!.Contains(SearchValue, StringComparison.InvariantCultureIgnoreCase))
            {
              return true;
            }
          }
          return false;
        }).AsQueryable();
      }
      //then field specific
      foreach (var field in VisibleFields)
      {
        if (FieldFilters.TryGetValue(field.Name, out var res))
        {
          set = from item in set
                let CellValue = field.Getter(item)
                let cellStringValue = CellValue == null ? string.Empty : CellValue.ToString()
                where cellStringValue.Contains(res, StringComparison.InvariantCultureIgnoreCase)
                select item;
        }
      }

      return set;
    }

    private string HandleClear(VisibleField<T> field)
    {
      if (string.IsNullOrWhiteSpace(GetFilterValue(field.Name)))
      {
        FieldFilters[field.Name] = string.Empty;
      }
      return FieldFilters[field.Name];
    }

    private void HandleSearchInput()
    {
      if (string.IsNullOrWhiteSpace(SearchValue))
      {
        SearchValue = string.Empty;
      }
    }

    private async void ExportToExcel()
    {
      var DataToExport = Selected.Count > 0 ? Selected : Data?.ToList();
      var res = ExcelUtilities.ExportToExcel(DataToExport!, VisibleFields);

      using var streamRef = new DotNetStreamReference(stream: res);
      
      await JSRuntime!.InvokeVoidAsync("downloadFileFromStream", (Context as BlazorGeneratorComponentBase).Title+".xlsx", streamRef);
    }
  }
}
