using BlazorEngine.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine.Components.DataGrid;

public partial class ListDataGrid<T>
{
  private IQueryable<T>? _cachedFilteredData;
  private int _lastDataHash;
  private object? _lastDataRef;
  private int _lastFieldFiltersHash;
  private string _lastSearchValue = string.Empty;

  private string _searchValue = string.Empty;

  private IQueryable<T>? FilteredData
  {
    get
    {
      var currentDataHash = Data?.GetHashCode() ?? 0;
      var currentFiltersHash = ComputeFiltersHash();

      if (_cachedFilteredData != null &&
          _lastSearchValue == _searchValue &&
          _lastFieldFiltersHash == currentFiltersHash &&
          ReferenceEquals(_lastDataRef, Data))
        return _cachedFilteredData;

      _cachedFilteredData = FilterDataInternal();
      _lastSearchValue = _searchValue;
      _lastFieldFiltersHash = currentFiltersHash;
      _lastDataRef = Data;
      return _cachedFilteredData;
    }
  }

  private FluentSearch? SearchBarRef { get; set; }
  private Dictionary<string, string> FieldFilters { get; } = [];

  private int ComputeFiltersHash()
  {
    var hash = new HashCode();
    foreach (var kvp in FieldFilters)
    {
      hash.Add(kvp.Key);
      hash.Add(kvp.Value);
    }

    return hash.ToHashCode();
  }

  private string GetFilterValue(string fieldName)
  {
    if (FieldFilters.TryGetValue(fieldName, out var res)) return res;
    FieldFilters[fieldName] = string.Empty;
    return string.Empty;
  }

  private void HandleFilter(ChangeEventArgs e, VisibleField<T> field)
  {
    if (e.Value is string value)
    {
      FieldFilters[field.Name] = value;
      InvalidateFilterCache();
    }
  }

  private void InvalidateFilterCache()
  {
    _cachedFilteredData = null;
  }

  private IQueryable<T>? FilterDataInternal()
  {
    if (Data is null) return null;

    IEnumerable<T> result = Data;

    // Global search
    if (!string.IsNullOrWhiteSpace(_searchValue))
    {
      var search = _searchValue;
      result = result.Where(r =>
      {
        foreach (var field in VisibleFields)
        {
          var cellValue = field.InternalGet(r);
          if (cellValue?.ToString()?.Contains(search, StringComparison.OrdinalIgnoreCase) == true) return true;
        }

        return false;
      });
    }

    // Field-specific filters
    foreach (var field in VisibleFields)
      if (FieldFilters.TryGetValue(field.Name, out var filterValue) &&
          !string.IsNullOrWhiteSpace(filterValue))
      {
        var capturedField = field;
        var capturedFilter = filterValue;
        result = result.Where(item =>
        {
          var cellValue = capturedField.InternalGet(item);
          return cellValue?.ToString()?.Contains(capturedFilter, StringComparison.OrdinalIgnoreCase) == true;
        });
      }

    return result.ToList().AsQueryable();
  }

  private string HandleClear(VisibleField<T> field)
  {
    if (string.IsNullOrWhiteSpace(GetFilterValue(field.Name))) FieldFilters[field.Name] = string.Empty;
    InvalidateFilterCache();
    return FieldFilters[field.Name];
  }

  private void HandleSearchInput()
  {
    if (string.IsNullOrWhiteSpace(_searchValue)) _searchValue = string.Empty;
    InvalidateFilterCache();
  }
}