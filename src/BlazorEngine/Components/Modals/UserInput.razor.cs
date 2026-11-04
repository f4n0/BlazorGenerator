using BlazorEngine.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine.Components.Modals;

public partial class UserInput : FluentDialogInstance
{
  [Parameter] public UserInputData Content { get; set; } = new();

  protected override Task OnActionClickedAsync(bool primary) =>
    primary ? DialogInstance.CloseAsync(Content) : DialogInstance.CancelAsync();
}
