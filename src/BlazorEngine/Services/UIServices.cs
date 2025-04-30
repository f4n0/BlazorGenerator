using BlazorEngine.Components.Modals;
using BlazorEngine.Layouts;
using BlazorEngine.Models;
using Microsoft.FluentUI.AspNetCore.Components;
using Newtonsoft.Json.Linq;

namespace BlazorEngine.Services
{
  public partial class UIServices(BlazorEngineLogger logger, IDialogService dialogService, ProgressService progressService, IKeyCodeService keyCodeService, LockUIService lockService)
  {
    public BlazorEngineLogger Logger { get; internal set; } = logger;
    public IDialogService DialogService { get; internal set; } = dialogService;
    public ProgressService ProgressService { get; internal set; } = progressService;
    public LockUIService LockService { get; internal set; } = lockService;
    public IKeyCodeService KeyCodeService { get; internal set; } = keyCodeService;

    public void StartLoader()
    {
      ProgressService.StartProgress();
    }
    public void StopLoader()
    {
      ProgressService.StopProgress();
    }
    public void LockUI()
    {
      LockService.LockUI();
    }
    public void UnlockUI()
    {
      LockService.UnlockUI();
    }

    public async Task<T?> OpenModal<T>(Type pageType, T data) where T : class
    {
      if (pageType.BaseType != typeof(CardPage<T>))
        throw new Exception("In order to use the modal, the pageType must have CardPage as baseType");

      var dialogResult = await DialogService.ShowDialogAsync(pageType, data, new DialogParameters()
      {
        Width = "50%",
        Height = "fit-content",
      }).ConfigureAwait(true);
      var result = await dialogResult.Result.ConfigureAwait(true);
      if ((result.Data is not null) && !result.Cancelled)
      {
        return result.Data as T;
      }
      return null;
    }

    public async Task<UploadFileData?> UploadFile(bool multiple = true, string fileFilters = "*.*", int maxFileCount = 50)
    {
      var data = new UploadFileData()
      {
        Multiple = multiple,
        FileFilters = fileFilters,
        MaximumFileCount = maxFileCount,
      };

      var dialogResult = await DialogService.ShowDialogAsync(typeof(FileInput), data, new DialogParameters()
      {
        Width = "50%",
        Height = "300px",
        PrimaryAction = "",
        SecondaryAction = "",
      }).ConfigureAwait(true);
      var result = await dialogResult.Result.ConfigureAwait(true);
      if ((result.Data is not null) && !result.Cancelled)
      {
        return result.Data as UploadFileData;
      }
      return null;
    }

    public async Task<UserInputData?> UserInput(UserInputData userInputData)
    {
      var dialogResult = await DialogService.ShowDialogAsync(typeof(UserInput), userInputData, new DialogParameters()
      {
        Width = "50%",
        Height = "fit-content",
      }).ConfigureAwait(true);
      var result = await dialogResult.Result.ConfigureAwait(true);
      if ((result.Data is not null) && !result.Cancelled)
      {
        return result.Data as UserInputData;
      }
      return null;
    }

    public async Task<T?> OpenPanel<T>(Type pageType, T data) where T : class
    {
      if (pageType.BaseType != typeof(CardPage<T>))
        throw new Exception("In order to use the modal, the pageType must have CardPage as baseType");

      var dialogResult = await DialogService.ShowPanelAsync(pageType, data, new DialogParameters()
      {
        DialogType = DialogType.Panel,
        Alignment = HorizontalAlignment.Right,
        Width = "40%",
      }).ConfigureAwait(true);
      var result = await dialogResult.Result.ConfigureAwait(true);
      if (result.Cancelled)
      {
        return null;
      }
      if (result.Data is not null)
      {
        return result.Data as T;
      }
      return null;
    }

    public async Task<T?> OpenPanel<T>(Type pageType, ModalData<T> data) where T : class
    {
      var dialogResult = await DialogService.ShowPanelAsync(pageType, data, new DialogParameters()
      {
        DialogType = DialogType.Panel,
        Alignment = HorizontalAlignment.Right,
        Width = "40%",
      }).ConfigureAwait(true);
      var result = await dialogResult.Result.ConfigureAwait(true);
      if ((result.Data is not null) && !result.Cancelled)
      {
        return (result.Data as ModalData<T>)?.Data;
      }
      return null;
    }
  }
}
