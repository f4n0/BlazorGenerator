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