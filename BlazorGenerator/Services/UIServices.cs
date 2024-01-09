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

  }
}
