namespace BlazorGenerator.Components.DataGrid;

public partial class ListDataGrid<T>
{
  internal Func<T, string> SelectedRowClass => (data) => Selected.IndexOf(data) != -1 ? "rowselected" : "";

  bool MultipleSelectEnabled = false;
  bool ShiftModifierEnabled = false;

  int LastSelectedIndex = 0;

  private void HandleSingleRecSelection(T? Rec, bool FromFirstColumn = false)
  {
    if (Rec == null || Data == null)
      return;

    if (!MultipleSelectEnabled && !ShiftModifierEnabled && !FromFirstColumn)
      Selected.Clear();


    int recIndex = Data.ToList().IndexOf(Rec);
    if (recIndex == -1)
      return;

    if (ShiftModifierEnabled)
    {
      int StartIndex = LastSelectedIndex;
      Selected.Clear();
      // Exit early if LastSelectedIndex is invalid.
      if (StartIndex == -1)
        return;


      // Ensure StartIndex is less than or equal to EndIndex.
      if (StartIndex > recIndex)
        (StartIndex, recIndex) = (recIndex, StartIndex);

      // Add range directly without using GetRange to avoid new list allocation.
      Selected.AddRange(Data.ToList().GetRange(StartIndex, (recIndex-StartIndex+1))); //(DataAsList.Skip(StartIndex).Take(recIndex - StartIndex + 1));
    }
    else
    {
      if ((MultipleSelectEnabled || FromFirstColumn) && Selected.Contains(Rec))
      {
        Selected.Remove(Rec);
      }
      else
      {
        Selected.Add(Rec);
        LastSelectedIndex = recIndex;
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