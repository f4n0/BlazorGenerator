using BlazorEngine.Components.Modals;
using BlazorEngine.Layouts;
using BlazorEngine.Models;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine.Services;

public partial class UIServices(
  BlazorEngineLogger logger,
  IDialogService dialogService,
  ProgressService progressService,
  IKeyCodeService keyCodeService,
  LockUIService lockService)
{
  public BlazorEngineLogger Logger { get; internal set; } = logger;
  public IDialogService DialogService { get; internal set; } = dialogService;
  public ProgressService ProgressService { get; internal set; } = progressService;
  public LockUIService LockService { get; internal set; } = lockService;
  public IKeyCodeService KeyCodeService { get; internal set; } = keyCodeService;

  public void StartLoader() => ProgressService.StartProgress();
  public void StopLoader() => ProgressService.StopProgress();
  public void LockUI() => LockService.LockUI();
  public void UnlockUI() => LockService.UnlockUI();

  public async Task<T?> OpenModal<T>(Type pageType, T data) where T : class
  {
    ValidateCardPage<T>(pageType);
    var options = CreateContentOptions(data, UISizeTokens.DialogWidth);
    options.Parameters[nameof(PageDialogContent.PageType)] = pageType;
    var result = await DialogService.ShowDialogAsync<PageDialogContent>(options);
    return result.Cancelled ? null : result.GetValue<T>();
  }

  public async Task<UploadFileData?> UploadFile(bool multiple = true, string fileFilters = "*.*", int maxFileCount = 50,
    long maxFileSize = 10 * 1024 * 1024)
  {
    var data = new UploadFileData
    {
      Multiple = multiple,
      FileFilters = fileFilters,
      MaximumFileCount = maxFileCount,
      MaximumFileSize = maxFileSize
    };
    var options = CreateContentOptions(data, UISizeTokens.DialogWidth);
    options.Header.Title = "Upload files";
    options.Height = UISizeTokens.UploadDialogHeight;
    options.Footer.PrimaryAction.Visible = false;
    options.Footer.SecondaryAction.Visible = false;
    var result = await DialogService.ShowDialogAsync<FileInput>(options);
    if (result.Cancelled) return null;
    var uploaded = result.GetValue<UploadFileData>();
    var error = uploaded?.Files.FirstOrDefault(file => !string.IsNullOrEmpty(file.ErrorMessage))?.ErrorMessage;
    if (!string.IsNullOrEmpty(error)) throw new Exception(error);
    return uploaded;
  }

  public async Task<UserInputData?> UserInput(UserInputData userInputData)
  {
    var result = await DialogService.ShowDialogAsync<UserInput>(
      CreateContentOptions(userInputData, UISizeTokens.DialogWidth));
    return result.Cancelled ? null : result.GetValue<UserInputData>();
  }

  public async Task<T?> OpenPanel<T>(Type pageType, T data) where T : class
  {
    ValidateCardPage<T>(pageType);
    var options = CreateContentOptions(data, UISizeTokens.PanelWidth);
    options.Parameters[nameof(PageDialogContent.PageType)] = pageType;
    var result = await DialogService.ShowDrawerAsync<PageDialogContent>(options);
    return result.Cancelled ? null : result.GetValue<T>();
  }

  public async Task<T?> OpenPanel<T>(Type pageType, ModalData<T> data) where T : class
  {
    var options = CreateContentOptions(data, UISizeTokens.PanelWidth);
    options.Parameters[nameof(PageDialogContent.PageType)] = pageType;
    var result = await DialogService.ShowDrawerAsync<PageDialogContent>(options);
    return result.Cancelled ? null : result.GetValue<ModalData<T>>()?.Data;
  }

  private static DialogOptions CreateContentOptions<T>(T content, string width)
  {
    var options = CreateDialogOptions(width);
    options.Parameters["Content"] = content;
    options.Footer.PrimaryAction.Label = "OK";
    options.Footer.PrimaryAction.OnClickAsync = instance => instance.CloseAsync(content);
    options.Footer.SecondaryAction.Label = Captions.Instance.Cancel;
    options.Footer.SecondaryAction.OnClickAsync = instance => instance.CancelAsync();
    return options;
  }

  internal static DialogOptions CreateDialogOptions(string width, string? height = "fit-content", string? title = null)
  {
    var options = new DialogOptions
    {
      Class = "blazorengine-dialog",
      Width = width,
      Height = height
    };
    options.Header.Title = title;
    options.Header.CloseAction.Visible = true;
    options.Header.CloseAction.Title = Captions.Instance.Close;
    options.Header.CloseAction.OnClickAsync = instance => instance.CancelAsync();
    return options;
  }

  private static void ValidateCardPage<T>(Type pageType)
  {
    if (!typeof(CardPage<T>).IsAssignableFrom(pageType))
      throw new ArgumentException("In order to use the modal, the pageType must inherit from CardPage.", nameof(pageType));
  }
}
