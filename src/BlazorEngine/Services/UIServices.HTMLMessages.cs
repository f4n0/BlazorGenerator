using Microsoft.AspNetCore.Components;
using BlazorEngine.Components.Modals;
using Microsoft.FluentUI.AspNetCore.Components;
using static Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size24;
using Color = Microsoft.FluentUI.AspNetCore.Components.Color;

namespace BlazorEngine.Services;

public partial class UIServices
{
  public Task<DialogResult> ShowSuccessAsync(string message, string? title = null) =>
    ShowMessageBoxAsync(CreateMessageOptions(message, title, "Success", new CheckmarkCircle(), Color.Success));

  public Task<DialogResult> ShowWarningAsync(string message, string? title = null) =>
    ShowMessageBoxAsync(CreateMessageOptions(message, title, "Warning", new Warning(), Color.Warning));

  public Task<DialogResult> ShowErrorAsync(string message, string? title = null) =>
    ShowMessageBoxAsync(CreateMessageOptions(message, title, "Error", new DismissCircle(), Color.Error));

  public Task<DialogResult> ShowInfoAsync(string message, string? title = null) =>
    ShowMessageBoxAsync(CreateMessageOptions(message, title, "Information", new Info(), Color.Default));

  public async Task<DialogResult> ShowConfirmationAsync(object receiver, Func<DialogResult, Task> callback,
    string message, string primaryText = "Yes", string secondaryText = "No", string? title = null)
  {
    var result = await ShowConfirmationAsync(message, primaryText, secondaryText, title);
    await EventCallback.Factory.Create(receiver, callback).InvokeAsync(result);
    return result;
  }

  public Task<DialogResult> ShowConfirmationAsync(string message, string primaryText = "Yes",
    string secondaryText = "No", string? title = null)
  {
    var options = CreateMessageOptions(message, title, "Confirm", new QuestionCircle(), Color.Primary);
    options.PrimaryButton = primaryText;
    options.SecondaryButton = secondaryText;
    return ShowMessageBoxAsync(options);
  }

  public Task<DialogResult> ShowMessageBoxAsync(MessageBoxOptions options)
  {
    var dialog = CreateDialogOptions("min(600px, calc(100vw - 2rem))", title: options.Title);
    dialog.Parameters[nameof(MessageBoxContent.Content)] = options;
    dialog.Footer.PrimaryAction.Label = options.PrimaryButton;
    dialog.Footer.PrimaryAction.ShortCut = options.PrimaryShortCut;
    dialog.Footer.SecondaryAction.Label = options.SecondaryButton;
    dialog.Footer.SecondaryAction.ShortCut = options.SecondaryShortCut;
    return DialogService.ShowDialogAsync<MessageBoxContent>(dialog);
  }

  private static MessageBoxOptions CreateMessageOptions(string message, string? title, string defaultTitle,
    Icon icon, Color color) => new()
  {
    Title = string.IsNullOrWhiteSpace(title) ? defaultTitle : title,
    Message = message,
    Icon = icon,
    IconColor = color,
    PrimaryButton = "OK"
  };
}
