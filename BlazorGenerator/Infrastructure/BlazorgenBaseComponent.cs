using BlazorGenerator.Dialogs;
using BlazorGenerator.Services;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Infrastructure
{
  public class BlazorgenBaseComponent : ComponentBase, IDisposable
  {

    [CascadingParameter]
    protected DynamicMainLayout layout { get; set; }
    [Inject]
    protected BlazorGenLogger logger { get; set; }
    [Inject]
    public IPageProgressService PageProgressService { get; set; }
    [Inject]
    public IMessageService MessageService { get; set; }
    [Inject]
    public IJSRuntime JSRuntime { get; set; }
    [Inject]
    public INotificationService NotificationService { get; set; }
    [Inject]
    public NavigationManager NavManager { get; set; }
    [Inject]
    public BlazorGenOptions Options { get; set; }


    public void setLogVisibility(bool show)
    {
      layout?.setLogVisibility(show);
    }

    public async Task<string> ShowChoose(string header, string[] options, string cancelText = "Cancel")
    {
      return await layout.ChooseService.ChoseAsync(header, options, cancelText);
    }

    public virtual string Title => this.GetType().Name;

    public void StartLoader()
    {
      PageProgressService.Go(null, options => { options.Color = Color.Danger; });
    }
    public void StopLoader()
    {
      PageProgressService.Go(-1);
    }




    #region Modal

    [Inject]
    public IModalService ModalService { get; set; }
    [Parameter]
    public bool IsModal { get; set; }

    TaskCompletionSource<object> AwaitModal;
    [Parameter] public Func<object, Task> ModalSuccess { get; set; }


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
      await ModalService.Hide();
      return returnData;
    }

    Task ModalData(object response)
    {
      AwaitModal.TrySetResult(response);
      return Task.CompletedTask;
    }

    public virtual void Dispose()
    {
      //GC.Collect();
    }

    protected override Task OnParametersSetAsync()
    {
      if (IsModal)
      {
        setLogVisibility(false);
      }
      return base.OnParametersSetAsync();
    }

    #endregion


    #region Files

    public Task<object> ShowFilePicker()
    {
      return OpenModalAsync<UploadFileDialog>();
    }
    #endregion

    public void SendLogMessage(string message, Enum.LogType logType = Enum.LogType.Info)
    {
      logger.SendLogMessage(message, logType);
    }


  }
}
