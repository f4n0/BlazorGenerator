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
