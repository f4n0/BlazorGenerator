

using BlazorEngine.Utils;

namespace BlazorEngine.Components.DataGrid;

public partial class ListDataGrid<T>
{
  internal Func<T, string> SelectedRowClass => (data) => Selected.IndexOf(data) != -1 ? "rowselected" : "";

  bool _multipleSelectEnabled = false;
  bool _shiftModifierEnabled = false;

  int _lastSelectedIndex = 0;

  private void HandleSingleRecSelection(T? rec, bool fromFirstColumn = false)
  {
    if (rec == null || Data == null)
      return;

    if (!_multipleSelectEnabled && !_shiftModifierEnabled && !fromFirstColumn)
      Selected.Clear();


    int recIndex = Data.ToList().IndexOf(rec);
    if (recIndex == -1)
      return;

    if (_shiftModifierEnabled)
    {
      int startIndex = _lastSelectedIndex;
      Selected.Clear();
      // Exit early if LastSelectedIndex is invalid.
      if (startIndex == -1)
        return;


      // Ensure StartIndex is less than or equal to EndIndex.
      if (startIndex > recIndex)
        (startIndex, recIndex) = (recIndex, startIndex);

      // Add range directly without using GetRange to avoid new list allocation.
      Selected.AddRange(Data.ToList().GetRange(startIndex, (recIndex-startIndex+1))); //(DataAsList.Skip(StartIndex).Take(recIndex - StartIndex + 1));
    }
    else
    {
      if ((_multipleSelectEnabled || fromFirstColumn) && Selected.Contains(rec))
      {
        Selected.Remove(rec);
      }
      else
      {
        Selected.Add(rec);
        _lastSelectedIndex = recIndex;
      }
    }

  }

  private bool? AllRecSelected
  {
    get =>
      Selected.Count == Data?.Count()
        ? true
        : Selected.Count == 0
          ? false
          : null;
    set
    {
      switch (value)
      {
        case true:
        {
          Selected.Clear();
          if (FilteredData != null)
          {
            Selected.AddRange(FilteredData);
          }

          break;
        }
        case false:
          Selected.Clear();
          break;
      }
    }
  }

}