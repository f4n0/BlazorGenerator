using System.ComponentModel.DataAnnotations;
using System.Reflection;
using BlazorEngine.Components.DataGrid;

namespace BlazorEngine.Tests;

public class DataGridSelectionTests
{
  [Fact]
  public void SelectionUsesCompositeModelEquality()
  {
    var first = new CompositeKeyItem(1, 1);
    var second = new CompositeKeyItem(1, 2);
    var grid = Activator.CreateInstance<ListDataGrid<CompositeKeyItem>>();

    var addToSelection = typeof(ListDataGrid<CompositeKeyItem>)
      .GetMethod("AddToSelection", BindingFlags.Instance | BindingFlags.NonPublic)!;
    var removeFromSelection = typeof(ListDataGrid<CompositeKeyItem>)
      .GetMethod("RemoveFromSelection", BindingFlags.Instance | BindingFlags.NonPublic)!;

    addToSelection.Invoke(grid, [first]);
    addToSelection.Invoke(grid, [second]);

    Assert.Equal(2, grid.Selected.Count);

    var replacement = new CompositeKeyItem(1, 1);
    removeFromSelection.Invoke(grid, [replacement]);

    Assert.Single(grid.Selected);
    Assert.Same(second, grid.Selected[0]);
  }

  [Fact]
  public void DataSourceChangeClearsPreviousSelection()
  {
    var first = new CompositeKeyItem(1, 1);
    var second = new CompositeKeyItem(1, 2);
    var grid = Activator.CreateInstance<ListDataGrid<CompositeKeyItem>>();
    var data = new List<CompositeKeyItem> { first };

    SetParameter(grid, "Data", data);
    grid.Selected.Add(first);
    InvokeOnParametersSet(grid);

    SetParameter(grid, "Data", new List<CompositeKeyItem> { second });
    InvokeOnParametersSet(grid);

    Assert.Empty(grid.Selected);
  }

  [Fact]
  public void SameDataSourceKeepsSelectionAcrossParameterUpdates()
  {
    var item = new CompositeKeyItem(1, 1);
    var data = new List<CompositeKeyItem> { item };
    var grid = Activator.CreateInstance<ListDataGrid<CompositeKeyItem>>();

    SetParameter(grid, "Data", data);
    grid.Selected.Add(item);
    InvokeOnParametersSet(grid);

    SetParameter(grid, "Data", data);
    InvokeOnParametersSet(grid);

    Assert.Single(grid.Selected);
  }

  private static void InvokeOnParametersSet(ListDataGrid<CompositeKeyItem> grid)
  {
    typeof(ListDataGrid<CompositeKeyItem>)
      .GetMethod("OnParametersSet", BindingFlags.Instance | BindingFlags.NonPublic)!
      .Invoke(grid, null);
  }

  private static void SetParameter(ListDataGrid<CompositeKeyItem> grid, string name, object value)
  {
    typeof(ListDataGrid<CompositeKeyItem>)
      .GetProperty(name, BindingFlags.Instance | BindingFlags.Public)!
      .SetValue(grid, value);
  }

  private sealed class CompositeKeyItem(int tenantId, int itemId) : IEquatable<CompositeKeyItem>
  {
    [Key] public int TenantId { get; } = tenantId;
    [Key] public int ItemId { get; } = itemId;

    public bool Equals(CompositeKeyItem? other)
      => other is not null && TenantId == other.TenantId && ItemId == other.ItemId;

    public override bool Equals(object? obj) => Equals(obj as CompositeKeyItem);

    // A hash collision must not make distinct composite keys compare equal.
    public override int GetHashCode() => 0;
  }
}
