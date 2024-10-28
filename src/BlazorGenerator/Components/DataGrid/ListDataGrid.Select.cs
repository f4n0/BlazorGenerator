namespace BlazorGenerator.Components.DataGrid;

public partial class ListDataGrid<T>
{
  internal Func<T, string> SelectedRowClass => (data) => Selected.Contains(data) ? "rowselected" : "";

  bool MultipleSelectEnabled = false;
  bool ShiftModifierEnabled = false;

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

  int LastSelectedIndex = 0;

  private void HandleSingleRecSelection(T? Rec)
  {
    if (!MultipleSelectEnabled && !ShiftModifierEnabled)
      Selected.Clear();
    if (Rec == null)
      return;

    var ListData = Data.ToList();

    if (ShiftModifierEnabled)
    {
      var StartIndex = LastSelectedIndex;
      var EndIndex = ListData.IndexOf(Rec);

      if (StartIndex == -1 || EndIndex == -1)
        return;

      Selected.Clear();

      if (StartIndex > EndIndex)
        (StartIndex, EndIndex) = (EndIndex, StartIndex);

      Selected = ListData.GetRange(StartIndex, (EndIndex - StartIndex) + 1);

    }
    else
    {
      if (Selected.Contains(Rec) && MultipleSelectEnabled)
      {
        Selected.Remove(Rec);
      }
      else if (Rec != null)
      {
        Selected.Add(Rec);
        LastSelectedIndex = ListData.IndexOf(Rec);
      }

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