using BlazorGenerator.Layouts;
using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Services
{
  public class UIServices(BlazorGenLogger _logger, IDialogService _dialogService, ProgressService _progressService)
  {
    public BlazorGenLogger Logger { get; internal set; } = _logger;
    public IDialogService DialogService { get; internal set; } = _dialogService;
    public ProgressService ProgressService { get; internal set; } = _progressService;

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

      var DialogResult = await DialogService.ShowPanelAsync(PageType, Data, new DialogParameters()
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
