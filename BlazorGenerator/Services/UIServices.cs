using BlazorGenerator.Infrastructure;
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
    public UIServices(IPageProgressService _PageProgressService, IMessageService _MessageService, BlazorGenLogger _logger, INotificationService _NotificationService, IModalService _ModalService)
    {
      PageProgressService = _PageProgressService;
      MessageService = _MessageService;
      logger = _logger;
      NotificationService = _NotificationService;
      ModalService = _ModalService;
    }

    public IPageProgressService PageProgressService { get; internal set; }
    public IMessageService MessageService { get; internal set; }
    public BlazorGenLogger logger { get; internal set; }
    public INotificationService NotificationService { get; internal set; }
    public IModalService ModalService { get; internal set; }



    TaskCompletionSource<object> AwaitModal;


    public void OpenModal<TComponent>()
    {
      Dictionary<string, object> parameters = new();
      OpenModal<TComponent>(parameters);
    }

    public void OpenModal<TComponent>(Dictionary<string, object> parameters)
    {
      parameters.Add(nameof(BlazorgenBaseComponent.IsModal), true);
      ModalService.Show<TComponent>(o =>
      {
        foreach (var item in parameters)
        {
          o.Add(item.Key, item.Value);
        }
      }, new ModalInstanceOptions
      {
        Width = Width.Max100,
        Size = ModalSize.ExtraLarge,

      });
    }

    public async Task<object> OpenModalAsync<TComponent>()
    {
      Dictionary<string, object> parameters = new();
      return await OpenModalAsync<TComponent>(parameters);
    }

    public async Task<object> OpenModalAsync<TComponent>(Dictionary<string, object> parameters)
    {
      parameters.Add(nameof(BlazorgenBaseComponent.IsModal), true);



      var Instance = await ModalService.Show<TComponent>(o =>
      {
        foreach (var item in parameters)
        {
          o.Add(item.Key, item.Value);
        }
        o.Add(nameof(BlazorgenBaseComponent.ModalSuccess), ModalData);
      }, new ModalInstanceOptions
      {

      });
      AwaitModal = new();
      var returnData = await AwaitModal.Task;
      Instance.ModalRef.Hide();
      return returnData;
    }

    Task ModalData(object response)
    {
      AwaitModal.TrySetResult(response);
      return Task.CompletedTask;
    }
  }
}
