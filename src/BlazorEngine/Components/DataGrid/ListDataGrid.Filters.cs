using BlazorEngine.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine.Components.DataGrid;

public partial class ListDataGrid<T>
{
  private IQueryable<T>? _cachedFilteredData;
    private string _lastSearchValue = string.Empty;
    private int _lastFieldFiltersHash;
    private int _lastDataHash;

    private IQueryable<T>? FilteredData
    {
        get
        {
            var currentDataHash = Data?.GetHashCode() ?? 0;
            var currentFiltersHash = ComputeFiltersHash();

            if (_cachedFilteredData != null &&
                _lastSearchValue == _searchValue &&
                _lastFieldFiltersHash == currentFiltersHash &&
                _lastDataHash == currentDataHash)
            {
                return _cachedFilteredData;
            }

            _cachedFilteredData = FilterDataInternal();
            _lastSearchValue = _searchValue;
            _lastFieldFiltersHash = currentFiltersHash;
            _lastDataHash = currentDataHash;
            return _cachedFilteredData;
        }
    }

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

    private string _searchValue = string.Empty;
    private FluentSearch? SearchBarRef { get; set; }
    private Dictionary<string, string> FieldFilters { get; set; } = [];

    private string GetFilterValue(string fieldName)
    {
        if (FieldFilters.TryGetValue(fieldName, out var res))
        {
            return res;
        }
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

        var list = Data.ToList(); // Materialize once

        // Global search
        if (!string.IsNullOrWhiteSpace(_searchValue))
        {
            list = list.Where(r =>
            {
                foreach (var field in VisibleFields)
                {
                    var cellValue = field.InternalGet(r);
                    if (cellValue?.ToString()?.Contains(_searchValue, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        return true;
                    }
                }
                return false;
            }).ToList();
        }

        // Field-specific filters
        foreach (var field in VisibleFields)
        {
            if (FieldFilters.TryGetValue(field.Name, out var filterValue) && 
                !string.IsNullOrWhiteSpace(filterValue))
            {
                list = list.Where(item =>
                {
                    var cellValue = field.InternalGet(item);
                    return cellValue?.ToString()?.Contains(filterValue, StringComparison.OrdinalIgnoreCase) == true;
                }).ToList();
            }
        }

        return list.AsQueryable();
    }

    private string HandleClear(VisibleField<T> field)
    {
        if (string.IsNullOrWhiteSpace(GetFilterValue(field.Name)))
        {
            FieldFilters[field.Name] = string.Empty;
        }
        InvalidateFilterCache();
        return FieldFilters[field.Name];
    }

    private void HandleSearchInput()
    {
        if (string.IsNullOrWhiteSpace(_searchValue))
        {
            _searchValue = string.Empty;
        }
        InvalidateFilterCache();
    }
}