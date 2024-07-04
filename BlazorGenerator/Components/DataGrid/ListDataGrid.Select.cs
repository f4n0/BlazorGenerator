namespace BlazorGenerator.Components.DataGrid;

public partial class ListDataGrid<T>
{
  internal Func<T, string> SelectedRowClass => (data) => Selected.Contains(data) ? "rowselected" : "";

  bool MultipleSelectEnabled = false;

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
    if (!MultipleSelectEnabled)
      Selected.Clear();

    if (Selected.Contains(Rec) && MultipleSelectEnabled)
    {
      Selected.Remove(Rec);
    }
    else if (Rec != null)
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
}