using BlazorGenerator.Components.Modals;
using BlazorGenerator.Layouts;
using BlazorGenerator.Models;
using Microsoft.FluentUI.AspNetCore.Components;
using Newtonsoft.Json.Linq;

namespace BlazorGenerator.Services
{
  public class UIServices(BlazorGenLogger _logger, IDialogService _dialogService, ProgressService _progressService, IKeyCodeService _keyCodeService)
  {
    public BlazorGenLogger Logger { get; internal set; } = _logger;
    public IDialogService DialogService { get; internal set; } = _dialogService;
    public ProgressService ProgressService { get; internal set; } = _progressService;
    public IKeyCodeService KeyCodeService { get; internal set; } = _keyCodeService;

    public void StartLoader()
    {
      ProgressService.StartProgress();
    }
    public void StopLoader()
    {
      ProgressService.StopProgress();
    }

    public async Task<T?> OpenModal<T>(Type PageType, T Data) where T : class
    {
      if (PageType.BaseType != typeof(CardPage<T>))
        throw new Exception("In order to use the modal, the pageType must have CardPage as baseType");

      var DialogResult = await DialogService.ShowDialogAsync(PageType, Data, new DialogParameters()
      {
        Width = "50%",
        Height = "fit-content"
      });
      var result = (await DialogResult.Result);
      if (result.Data is not null)
      {
        return result.Data as T;
      }
      else
      {
        return null;
      }
    }


    public async Task<UploadFileData?> UploadFile(bool multiple = true, string filefilters = "*.*", int maxfilecount = 50) 
    {
      var data = new UploadFileData()
      {
        Multiple = multiple,
        FileFilters = filefilters,
        MaximumFileCount = maxfilecount
      };

      var DialogResult = await DialogService.ShowDialogAsync(typeof(FileInput), data, new DialogParameters()
      {
        Width = "50%",
        Height = "300px",
        PrimaryAction = "",
        SecondaryAction = ""        
      }) ;
      var result = (await DialogResult.Result);
      if (result.Data is not null)
      {
        return result.Data as UploadFileData;
      }
      else
      {
        return null;
      }
    }

    public async Task<UserInputData?> UserInput(UserInputData userInputData)
    {
      var DialogResult = await DialogService.ShowDialogAsync(typeof(UserInput), userInputData, new DialogParameters()
      {
        Width = "50%",
        Height = "fit-content"
      });
      var result = (await DialogResult.Result);
      if (result.Data is not null)
      {
        return result.Data as UserInputData;
      }
      else
      {
        return null;
      }
    }

    public async Task<T?> OpenPanel<T>(Type PageType, T Data) where T : class
    {
      if (PageType.BaseType != typeof(CardPage<T>))
        throw new Exception("In order to use the modal, the pageType must have CardPage as baseType");

      var Original = Data;
      if(Data is ICloneable cloneable)
      {
        Original = (T) cloneable.Clone();
      } else
      {
        Original = JObject.Parse(JObject.FromObject(Data).ToString()).ToObject<T>();
      }

      var DialogResult = await DialogService.ShowPanelAsync(PageType, Data, new DialogParameters()
      {
        DialogType = DialogType.Panel,
        Alignment = HorizontalAlignment.Right,
        Width = "40%"
      });
      var result = (await DialogResult.Result);
      if(result.Cancelled)
      {
        return Original;
      }
      if (result.Data is not null)
      {
        return result.Data as T;
      }
      else
      {
        return null;
      }
    }

    public async Task<T?> OpenPanel<T>(Type PageType, ModalData<T> Data) where T : class
    {
      var DialogResult = await DialogService.ShowPanelAsync(PageType, Data, new DialogParameters()
      {
        DialogType = DialogType.Panel,
        Alignment = HorizontalAlignment.Right,
        Width = "40%"
      });
      var result = (await DialogResult.Result);
      if (result.Data is not null)
      {
        return (result.Data as ModalData<T>)?.Data;
      }
      else
      {
        return null;
      }
    }
  }
}
