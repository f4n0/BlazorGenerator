using BlazorEngine.Actions;
using Microsoft.AspNetCore.Components;

namespace BlazorEngine.Components.PageAction;

public partial class ActionBar
{
  [Parameter] public IReadOnlyList<PageActionDefinition> PageActions { get; set; } = [];

  [Parameter] public required object Context { get; set; }

  public Dictionary<string, int> ActionGroups { get; set; } = [];

  public Dictionary<string, bool> PopOverBind { get; set; } = [];

  public bool OpenMore { get; set; }

  private List<ActionGroup> _overflowGroups = [];

  protected override void OnParametersSet()
  {
    PopulateDictionary();
    _overflowGroups = [];
    foreach (var group in PageActions.GroupBy(action => action.Group))
    {
      var actions = group.ToList();
      if (group.Key.Equals("default", StringComparison.OrdinalIgnoreCase))
        _overflowGroups.AddRange(actions.Select(action => new ActionGroup(group.Key, [action])));
      else
        _overflowGroups.Add(new ActionGroup(group.Key, actions));
    }
    base.OnParametersSet();
  }

  private sealed record ActionGroup(string Name, List<PageActionDefinition> Actions);

  private static Task InvokeActionAsync(PageActionDefinition action)
  {
    return action.ExecuteAsync();
  }

  private Task InvokeOverflowActionAsync(PageActionDefinition action)
  {
    OpenMore = false;
    return InvokeActionAsync(action);
  }

  private void PopulateDictionary()
  {
    ActionGroups = [];
    foreach (var item in PageActions)
      if (ActionGroups.TryGetValue(item.Group, out var value))
        ActionGroups[item.Group] = ++value;
      else
        ActionGroups.Add(item.Group, 1);
  }
}
