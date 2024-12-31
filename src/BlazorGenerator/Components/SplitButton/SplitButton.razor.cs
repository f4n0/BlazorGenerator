using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator.Components.SplitButton;
public partial class SplitButton
{
  bool OpenMenu = false;
  string id = Identifier.NewId();


  [Parameter]
  public RenderFragment? ChildContent { get; set; }

  [Parameter]
  public string? Title { get; set; }

  [Parameter]
  public EventCallback<MouseEventArgs> OnClick { get; set; }


  [Parameter]
  public Appearance Appearance { get; set; }

  [Parameter]
  public Icon? Icon { get; set; }
}
