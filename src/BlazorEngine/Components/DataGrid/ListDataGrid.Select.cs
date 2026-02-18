

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

    var dataList = Data.ToList();

    if (!_multipleSelectEnabled && !_shiftModifierEnabled && !fromFirstColumn)
      Selected.Clear();

    int recIndex = dataList.IndexOf(rec);
    if (recIndex == -1)
      return;

    if (_shiftModifierEnabled)
    {
      int startIndex = _lastSelectedIndex;
      Selected.Clear();
      if (startIndex == -1)
        return;

      if (startIndex > recIndex)
        (startIndex, recIndex) = (recIndex, startIndex);

      Selected.AddRange(dataList.Skip(startIndex).Take(recIndex - startIndex + 1));
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