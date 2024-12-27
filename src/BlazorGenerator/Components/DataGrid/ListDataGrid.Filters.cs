using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator.Components.DataGrid;

public partial class ListDataGrid<T>
{
  private IQueryable<T>? FilteredData => FilterData();
  private string SearchValue = string.Empty;
  private FluentSearch? SearchBarRef { get; set; }
  private Dictionary<string, string> FieldFilters { get; set; } = [];

  private string GetFilterValue(string fieldName)
  {
    if (FieldFilters.TryGetValue(fieldName, out var res))
    {
      return res;
    }
    FieldFilters.Add(fieldName, string.Empty);
    return string.Empty;
  }

  private void HandleFilter(ChangeEventArgs e, VisibleField<T> field)
  {
    if (e.Value is string value)
    {
      FieldFilters[field.Name] = value;
    }
  }

  private IQueryable<T>? FilterData()
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
          var CellValue = field.InternalGet(r);
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
              let CellValue = field.InternalGet(item)
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
}