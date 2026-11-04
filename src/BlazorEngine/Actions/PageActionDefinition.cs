using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Filled;

namespace BlazorEngine.Actions;

public sealed record PageActionDefinition
{
  private static readonly Icon DefaultIcon = new Size20.Run();

  public string Caption { get; }
  public string Group { get; }
  public Icon Icon { get; }
  public Func<Task> ExecuteAsync { get; }

  public PageActionDefinition(string caption, Action execute, string group = "Default", Icon? icon = null)
    : this(caption, () =>
    {
      execute();
      return Task.CompletedTask;
    }, group, icon)
  {
  }

  public PageActionDefinition(string caption, Func<Task> executeAsync, string group = "Default", Icon? icon = null)
  {
    Caption = caption;
    Group = group;
    Icon = icon ?? DefaultIcon;
    ExecuteAsync = executeAsync;
  }
}
