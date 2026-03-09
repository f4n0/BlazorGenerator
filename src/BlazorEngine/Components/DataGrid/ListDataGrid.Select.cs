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

  private void RefreshSelectionSnapshot()
  {
    _selectedSnapshot = [.. Selected];
    UpdateSelectAllState();
  }

  private void UpdateSelectAllState()
  {
    var totalCount = FilteredData?.Count() ?? 0;
    var selectedCount = Selected.Count;

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
    if (args.Selected)
    {
      if (!Selected.Contains(args.Item))
        Selected.Add(args.Item);
    }
    else
    {
      Selected.Remove(args.Item);
    }

    SelectedChanged.InvokeAsync(Selected);
    RefreshSelectionSnapshot();
  }

  /// <summary>
  ///   Handles the "Select All" checkbox in the SelectColumn header.
  /// </summary>
  private void HandleSelectAllChanged(bool? selectAll)
  {
    Selected.Clear();
    if (selectAll == true && FilteredData != null)
      foreach (var item in FilteredData)
        Selected.Add(item);

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
      if (Selected.Contains(cell.Item))
        Selected.Remove(cell.Item);
      else
        Selected.Add(cell.Item);

      _anchorItem = cell.Item;
    }
    else if (_shiftPressed)
    {
      // Shift+click: range select from anchor to clicked item
      if (_anchorItem != null && FilteredData != null)
      {
        var items = FilteredData.ToList();
        var anchorIndex = items.IndexOf(_anchorItem);
        var clickedIndex = items.IndexOf(cell.Item);

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