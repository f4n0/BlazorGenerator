using BlazorGenerator.Layouts;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Services
{
  public class UIServices
  {
    public UIServices(BlazorGenLogger _logger, IDialogService _dialogService, ProgressService? _progressService)
    {
      //PageProgressService = _PageProgressService;
      //MessageService = _MessageService;
      logger = _logger;
      //NotificationService = _NotificationService;
      //ModalService = _ModalService;
      dialogService = _dialogService;
      progressService = _progressService;
    }


    public BlazorGenLogger logger { get; internal set; }
    public IDialogService dialogService { get; internal set; }
    public ProgressService progressService { get; internal set; }

    public void StartLoader()
    {
      progressService.StartProgress();
    }
    public void StopLoader()
    {
      progressService.StopProgress();
    }

    public async Task<T?> OpenModal<T>(Type PageType, T Data) where T : class
    {
      if (PageType.BaseType != typeof(CardPage<T>))
        throw new Exception("In order to use the modal, the pageType must have CardPage as baseType");

      var DialogResult = await dialogService.ShowDialogAsync(PageType, Data, new DialogParameters()
      {
        Width = "50%",
        Height = "50%"
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

    public async Task<T?> OpenPanel<T>(Type PageType, T Data) where T : class
    {
      if (PageType.BaseType != typeof(CardPage<T>))
        throw new Exception("In order to use the modal, the pageType must have CardPage as baseType");

      var DialogResult = await dialogService.ShowPanelAsync(PageType, Data, new DialogParameters()
      {
        DialogType = DialogType.Panel,
        Alignment = HorizontalAlignment.Right,
        Width = "40%"
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

  }
}
