

namespace BlazorEngine.Components.DataGrid;

public partial class ListDataGrid<T>
{
  /// <summary>
  /// Handles individual row selection/deselection from the SelectColumn component.
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
  }

  /// <summary>
  /// Handles the "Select All" checkbox in the SelectColumn header.
  /// </summary>
  private void HandleSelectAllChanged(bool? selectAll)
  {
    Selected.Clear();
    if (selectAll == true && FilteredData != null)
    {
      foreach (var item in FilteredData)
      {
        Selected.Add(item);
      }
    }
    SelectedChanged.InvokeAsync(Selected);
  }
}