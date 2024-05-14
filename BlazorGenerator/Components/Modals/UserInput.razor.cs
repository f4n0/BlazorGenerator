using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator.Components.Modals
{
  public partial class UserInput : IDialogContentComponent<UserInputData>
  {
    [Parameter]
    public UserInputData Content { get; set; } = new();
  }
}
