using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using static Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size24;
using Color = Microsoft.FluentUI.AspNetCore.Components.Color;

namespace BlazorEngine.Services;

public partial class UIServices
{
    /// <summary>
  ///   Shows a success message box. Does not have a callback function.
  /// </summary>
  /// <param name="message">The message to display.</param>
  /// <param name="title">The title to display on the dialog.</param>
  public async Task<IDialogReference> ShowSuccessAsync(string message, string? title = null)
  {
    return await ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>
    {
      Content = new MessageBoxContent
      {
        Title = string.IsNullOrWhiteSpace(title) ? "Success" : title,
        Intent = MessageBoxIntent.Success,
        Icon = new CheckmarkCircle(),
        IconColor = Color.Success,
        MarkupMessage = (MarkupString)message
      },
      DialogType = DialogType.MessageBox,
      PrimaryAction = "OK",
      SecondaryAction = string.Empty
    }).ConfigureAwait(true);
  }

  /// <summary>
  ///   Shows a warning message box. Does not have a callback function.
  /// </summary>
  /// <param name="message">The message to display.</param>
  /// <param name="title">The title to display on the dialog.</param>
  public async Task<IDialogReference> ShowWarningAsync(string message, string? title = null)
  {
    return await ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>
    {
      Content = new MessageBoxContent
      {
        Title = string.IsNullOrWhiteSpace(title) ? "Warning" : title,
        Intent = MessageBoxIntent.Warning,
        Icon = new Warning(),
        IconColor = Color.Warning,
        MarkupMessage = (MarkupString)message
      },
      DialogType = DialogType.MessageBox,
      PrimaryAction = "OK",
      SecondaryAction = string.Empty
    }).ConfigureAwait(true);
  }

  /// <summary>
  ///   Shows an error message box. Does not have a callback function.
  /// </summary>
  /// <param name="message">The message to display.</param>
  /// <param name="title">The title to display on the dialog.</param>
  public async Task<IDialogReference> ShowErrorAsync(string message, string? title = null)
  {
    return await ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>
    {
      Content = new MessageBoxContent
      {
        Title = string.IsNullOrWhiteSpace(title) ? "Error" : title,
        Intent = MessageBoxIntent.Error,
        Icon = new DismissCircle(),
        IconColor = Color.Error,
        MarkupMessage = (MarkupString)message
      },
      DialogType = DialogType.MessageBox,
      PrimaryAction = "OK",
      SecondaryAction = string.Empty
    }).ConfigureAwait(true);
  }

  /// <summary>
  ///   Shows an information message box. Does not have a callback function.
  /// </summary>
  /// <param name="message">The message to display.</param>
  /// <param name="title">The title to display on the dialog.</param>
  public async Task<IDialogReference> ShowInfoAsync(string message, string? title = null)
  {
    return await ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>
    {
      Content = new MessageBoxContent
      {
        Title = string.IsNullOrWhiteSpace(title) ? "Information" : title,
        Intent = MessageBoxIntent.Info,
        Icon = new Info(),
        IconColor = Color.Info,
        MarkupMessage = (MarkupString)message
      },
      DialogType = DialogType.MessageBox,
      PrimaryAction = "OK",
      SecondaryAction = string.Empty
    }).ConfigureAwait(true);
  }

  /// <summary>
  ///   Shows a confirmation message box. Has a callback function which returns boolean
  ///   (true=PrimaryAction clicked, false=SecondaryAction clicked).
  /// </summary>
  /// <param name="receiver">The component that receives the callback function.</param>
  /// <param name="callback">The callback function.</param>
  /// <param name="message">The message to display.</param>
  /// <param name="primaryText">The text to display on the primary button.</param>
  /// <param name="secondaryText">The text to display on the secondary button.</param>
  /// <param name="title">The title to display on the dialog.</param>
  public async Task<IDialogReference> ShowConfirmationAsync(object receiver, Func<DialogResult, Task> callback,
    string message, string primaryText = "Yes", string secondaryText = "No", string? title = null)
  {
    return await ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>
    {
      Content = new MessageBoxContent
      {
        Title = string.IsNullOrWhiteSpace(title) ? "Confirm" : title,
        Intent = MessageBoxIntent.Confirmation,
        Icon = new QuestionCircle(),
        IconColor = Color.Success,
        MarkupMessage = (MarkupString)message
      },
      DialogType = DialogType.MessageBox,
      PrimaryAction = primaryText,
      SecondaryAction = secondaryText,
      OnDialogResult = EventCallback.Factory.Create(receiver, callback)
    }).ConfigureAwait(true);
  }

  /// <summary>
  ///   Shows a confirmation message box. Has no callback function
  ///   (true=PrimaryAction clicked, false=SecondaryAction clicked).
  /// </summary>
  /// <param name="message">The message to display.</param>
  /// <param name="primaryText">The text to display on the primary button.</param>
  /// <param name="secondaryText">The text to display on the secondary button.</param>
  /// <param name="title">The title to display on the dialog.</param>
  public async Task<IDialogReference> ShowConfirmationAsync(string message, string primaryText = "Yes",
    string secondaryText = "No", string? title = null)
  {
    return await ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>
    {
      Content = new MessageBoxContent
      {
        Title = string.IsNullOrWhiteSpace(title) ? "Confirm" : title,
        Intent = MessageBoxIntent.Confirmation,
        Icon = new QuestionCircle(),
        IconColor = Color.Success,
        MarkupMessage = (MarkupString)message
      },
      DialogType = DialogType.MessageBox,
      PrimaryAction = primaryText,
      SecondaryAction = secondaryText
    }).ConfigureAwait(true);
  }

  /// <summary>
  ///   Shows a custom message box. Has a callback function which returns boolean
  ///   (true=PrimaryAction clicked, false=SecondaryAction clicked).
  /// </summary>
  /// <param name="parameters">Parameters to pass to component being displayed.</param>
  public async Task<IDialogReference> ShowMessageBoxAsync(DialogParameters<MessageBoxContent> parameters)
  {
    DialogParameters dialogParameters = new()
    {
      DialogType = DialogType.MessageBox,
      Alignment = HorizontalAlignment.Center,
      Title = parameters.Content.Title,
      Modal = string.IsNullOrEmpty(parameters.SecondaryAction),
      ShowDismiss = false,
      PrimaryAction = parameters.PrimaryAction,
      SecondaryAction = parameters.SecondaryAction,
      Width = parameters.Width,
      Height = parameters.Height,
      AriaLabel = $"{parameters.Content.Title}",
      OnDialogResult = parameters.OnDialogResult
    };

    return await DialogService.ShowDialogAsync(typeof(MessageBox), parameters.Content, dialogParameters)
      .ConfigureAwait(true);
  }
}