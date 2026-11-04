using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace BlazorEngine.Components.SplitButton;

public partial class SplitButton
{
  private readonly string _id = Identifier.NewId();


  [Parameter] public RenderFragment? ChildContent { get; set; }

  [Parameter] public string? Title { get; set; }

  [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }


  [Parameter] public ButtonAppearance Appearance { get; set; }

  [Parameter] public Icon? Icon { get; set; }
}
