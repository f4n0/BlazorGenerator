using BlazorGenerator.Attributes;
using BlazorGenerator.Models;
using BlazorGenerator.Services;
using BlazorGenerator.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Reflection;

namespace BlazorGenerator.Layouts.Partial
{
  public partial class ListDataGrid<T> where T : class
  {
    [Inject]
    public UIServices? UIServices { get; set; }

    [Parameter]
    public object Context { get; set; }

    [Parameter]
    public List<VisibleField<T>> VisibleFields { get; set; }

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

    internal T? CurrRec { get; set; }

    [Parameter]
    public virtual Type? EditFormType { get; set; }

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
    private void HandleSingleRecSelection(T Rec)
    {
      Selected.Clear();
      Selected.Add(Rec);
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
          Selected.AddRange(FilteredData);
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
      StateHasChanged();
    }

    [Parameter]
    public Action<T>? OnDiscard { get; set; }

    protected void HandleDiscard(T Data)
    {
      OnDiscard?.Invoke(Data);
      StateHasChanged();
    }

    [Parameter]
    public Func<T>? OnNewItem { get; set; }


    protected async void NewItem()
    {
      var item = OnNewItem?.Invoke();
      if (item is null)
        item = Activator.CreateInstance<T>();
      await EditAsync(item);
      var datalist = Data.ToList();
      datalist.Add(item);
      Data = datalist.AsQueryable();
      StateHasChanged();
    }

    string GetCssGridTemplate(int GridActions, PermissionSet? permissionSet)
    {
      const string select = "50px ";
      string actions = string.Empty;
      if (GridActions > 0)
        actions = "50px ";

      var spacing = 70 / VisibleFields.Count;
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

  }

}
