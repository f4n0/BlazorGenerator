using System.Reflection;
using BlazorEngine.Attributes;
using BlazorEngine.Utils;
using Microsoft.AspNetCore.Components;

namespace BlazorEngine.Components.PageAction;

public partial class ActionBar
{
  [Parameter] public IEnumerable<(MethodInfo Method, PageActionAttribute Attribute)> PageActions { get; set; } = [];

  [Parameter] public required object Context { get; set; }

  public Dictionary<string, int> ActionGroups { get; set; } = [];

  public Dictionary<string, bool> PopOverBind { get; set; } = [];

  public bool OpenMore { get; set; }

  private List<ActionGroup> _overflowGroups = [];

  protected override void OnParametersSet()
  {
    PopulateDictionary();
    _overflowGroups = [];
    foreach (var group in PageActions.GroupBy(action => action.Attribute.Group))
    {
      var actions = group.ToList();
      if (group.Key.Equals("default", StringComparison.OrdinalIgnoreCase))
        _overflowGroups.AddRange(actions.Select(action => new ActionGroup(group.Key, [action])));
      else
        _overflowGroups.Add(new ActionGroup(group.Key, actions));
    }
    base.OnParametersSet();
  }

  private sealed record ActionGroup(string Name, List<(MethodInfo Method, PageActionAttribute Attribute)> Actions);

  private Task InvokeActionAsync(MethodInfo method)
  {
    return ReflectionUtilites.InvokeAction(method, Context);
  }

  private Task InvokeOverflowActionAsync(MethodInfo method)
  {
    OpenMore = false;
    return InvokeActionAsync(method);
  }

  private void PopulateDictionary()
  {
    ActionGroups = [];
    foreach (var item in PageActions)
      if (ActionGroups.TryGetValue(item.Attribute.Group, out var value))
        ActionGroups[item.Attribute.Group] = ++value;
      else
        ActionGroups.Add(item.Attribute.Group, 1);
  }
}
