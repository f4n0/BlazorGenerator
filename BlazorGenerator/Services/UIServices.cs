using Blazorise;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Services
{
  public class UIServices
  {
    public UIServices(IPageProgressService _PageProgressService, IMessageService _MessageService, BlazorGenLogger _logger, INotificationService _NotificationService)
    {
      PageProgressService = _PageProgressService;
      MessageService = _MessageService;
      logger = _logger;
      NotificationService = _NotificationService;
    }

    public IPageProgressService PageProgressService { get; private set; }
    public IMessageService MessageService { get; private set; }
    public BlazorGenLogger logger { get; private set; }
    public INotificationService NotificationService { get; private set; }
  }
}
