using System.ComponentModel.DataAnnotations;
using System.Reflection;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine.Components.DataGrid;

public partial class ListDataGrid<T>
{
  /// <summary>
  ///   Tracks whether Ctrl key is currently held down.
  /// </summary>
  private bool _ctrlPressed;

  private bool _shiftPressed = false;

  /// <summary>
  ///   Tracks the SelectAll checkbox state: true = all, false = none, null = indeterminate.
  /// </summary>
  private bool? _selectAll = false;

  /// <summary>
  ///   Tracks the last individually clicked item, used as the anchor for Shift+click range selection.
  /// </summary>
  private T? _anchorItem;

  /// <summary>
  ///   A snapshot list whose reference changes whenever selection changes,
  ///   forcing SelectColumn to re-evaluate.
  /// </summary>
  private List<T> _selectedSnapshot = [];

  private static readonly Func<T, object?>? ItemKeySelector = CreateItemKeySelector();

  private static Func<T, object?>? CreateItemKeySelector()
  {
    var keyProperty = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
      .FirstOrDefault(static property => Attribute.IsDefined(property, typeof(KeyAttribute)))
      ?? typeof(T).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

    return keyProperty == null ? null : item => keyProperty.GetValue(item);
  }

  private static bool ItemsMatch(T left, T right)
  {
    if (ItemKeySelector != null)
    {
      var leftKey = ItemKeySelector(left);
      var rightKey = ItemKeySelector(right);
      if (leftKey != null && rightKey != null)
        return Equals(leftKey, rightKey);
    }

    return EqualityComparer<T>.Default.Equals(left, right);
  }

  private bool SelectionContains(T item)
  {
    return Selected.Any(selectedItem => ItemsMatch(selectedItem, item));
  }

  private void AddToSelection(T item)
  {
    if (!SelectionContains(item))
      Selected.Add(item);
  }

  private void RemoveFromSelection(T item)
  {
    var selectedItem = Selected.FirstOrDefault(existingItem => ItemsMatch(existingItem, item));
    if (selectedItem != null)
      Selected.Remove(selectedItem);
  }

  private bool IsVisibleItem(T item)
  {
    return FilteredItems.Any(visibleItem => ItemsMatch(visibleItem, item));
  }

  private void RefreshSelectionSnapshot()
  {
    RefreshSelectionSnapshot(FilteredItems);
  }

  private void RefreshSelectionSnapshot(IReadOnlyCollection<T> visibleItems)
  {
    _selectedSnapshot = [.. visibleItems.Where(SelectionContains)];
    UpdateSelectAllState(visibleItems);
  }

  private void UpdateSelectAllState()
  {
    UpdateSelectAllState(FilteredItems);
  }

  private void UpdateSelectAllState(IReadOnlyCollection<T> visibleItems)
  {
    var totalCount = visibleItems.Count;
    var selectedCount = visibleItems.Count(SelectionContains);

    if (selectedCount == 0)
      _selectAll = false;
    else if (totalCount > 0 && selectedCount >= totalCount)
      _selectAll = true;
    else
      _selectAll = null; // indeterminate
  }

  /// <summary>
  ///   Handles individual row selection/deselection from the SelectColumn checkbox.
  ///   Always acts as multi-select toggle (add/remove without clearing others).
  /// </summary>
  private void HandleSelectionChange((T Item, bool Selected) args)
  {
    if (!args.Selected && !IsVisibleItem(args.Item))
    {
      RefreshSelectionSnapshot();
      return;
    }

    if (args.Selected)
    {
      AddToSelection(args.Item);
    }
    else
    {
      RemoveFromSelection(args.Item);
    }

    SelectedChanged.InvokeAsync(Selected);
    RefreshSelectionSnapshot();
  }

  /// <summary>
  ///   Handles the "Select All" checkbox in the SelectColumn header.
  /// </summary>
  private void HandleSelectAllChanged(bool? selectAll)
  {
    if (selectAll == true)
    {
      foreach (var visibleItem in FilteredItems)
        AddToSelection(visibleItem);
    }
    else
    {
      foreach (var visibleItem in FilteredItems.Where(SelectionContains).ToList())
        RemoveFromSelection(visibleItem);
    }

    SelectedChanged.InvokeAsync(Selected);
    RefreshSelectionSnapshot();
  }

  /// <summary>
  ///   Handles cell click on the grid.
  ///   Ignores clicks on the SelectColumn (those are handled by OnSelect).
  ///   Normal click = single select. Ctrl+click = multi-select toggle.
  /// </summary>
  private void HandleCellClick(FluentDataGridCell<T> cell)
  {
    // Skip clicks on the SelectColumn — already handled by HandleSelectionChange
    if (cell.Column is SelectColumn<T>)
      return;
    

    if (cell.Item == null)
      return;

    if (_ctrlPressed)
    {
      // Ctrl+click: multi-select toggle
      if (SelectionContains(cell.Item))
        RemoveFromSelection(cell.Item);
      else
        AddToSelection(cell.Item);

      _anchorItem = cell.Item;
    }
    else if (_shiftPressed)
    {
      // Shift+click: range select from anchor to clicked item
      if (_anchorItem != null && FilteredItems.Count > 0)
      {
        var items = FilteredItems;
        var anchorIndex = items.FindIndex(item => ItemsMatch(item, _anchorItem));
        var clickedIndex = items.FindIndex(item => ItemsMatch(item, cell.Item));

        if (anchorIndex >= 0 && clickedIndex >= 0)
        {
          var start = Math.Min(anchorIndex, clickedIndex);
          var end = Math.Max(anchorIndex, clickedIndex);

          Selected.Clear();
          for (var i = start; i <= end; i++)
            Selected.Add(items[i]);
        }
      }
      else
      {
        // No anchor yet, treat as single select
        Selected.Clear();
        Selected.Add(cell.Item);
        _anchorItem = cell.Item;
      }
    }
    else
    {
      // Normal click: single select
      Selected.Clear();
      Selected.Add(cell.Item);
      _anchorItem = cell.Item;
    }

    SelectedChanged.InvokeAsync(Selected);
    RefreshSelectionSnapshot();
  }
}
