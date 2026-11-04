using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Filled;

namespace BlazorEngine.Actions;

public sealed record GridActionDefinition<TRow>
{
  private static readonly Icon DefaultIcon = new Size20.Run();

  public string Caption { get; }
  public Icon Icon { get; }
  public Func<TRow, Task> ExecuteAsync { get; }

  public GridActionDefinition(string caption, Action<TRow> execute, Icon? icon = null)
    : this(caption, row =>
    {
      execute(row);
      return Task.CompletedTask;
    }, icon)
  {
  }

  public GridActionDefinition(string caption, Func<TRow, Task> executeAsync, Icon? icon = null)
  {
    Caption = caption;
    Icon = icon ?? DefaultIcon;
    ExecuteAsync = executeAsync;
  }
}
