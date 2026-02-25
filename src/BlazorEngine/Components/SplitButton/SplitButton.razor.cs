using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine.Components.SplitButton;

public partial class SplitButton
{
  private readonly string _id = Identifier.NewId();
  private bool _openMenu;


  [Parameter] public RenderFragment? ChildContent { get; set; }

  [Parameter] public string? Title { get; set; }

  [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }


  [Parameter] public Appearance Appearance { get; set; }

  [Parameter] public Icon? Icon { get; set; }
}