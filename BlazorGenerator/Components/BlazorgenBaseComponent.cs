using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Components
{
  public class BlazorgenBaseComponent: ComponentBase
  {
    [CascadingParameter]
    protected DynamicMainLayout layout { get; set; }

    public void setLogVisibility(bool show)
    {
      layout.setLogVisibility(show);
    }

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

    public virtual string Title => "";


    public void StartLoader()
    {
      PageProgressService.Go(null, options => { options.Color = Color.Danger; });
    }
    public void StopLoader()
    {
      PageProgressService.Go(-1);
    }

    internal Modal ModalRef;
    internal RenderFragment ChildModalContent;
    internal ModalSize modalSize = ModalSize.ExtraLarge;


    public void InitModal<TModalType, TModalData>(object ModalData) where TModalType : ModalPage<TModalData>
    {
      ChildModalContent = new RenderFragment(builder => {
        builder.OpenComponent<TModalType>(5);
        builder.AddAttribute(6, "Data", ModalData);
        builder.AddAttribute(7, "onSave", EventCallback.Factory.Create<object>(this, ModalCallback));
        builder.CloseComponent();
      });

      StateHasChanged();
    }

    // Summary:
    //     Specifies the types of files that the input accepts. https://www.w3schools.com/tags/att_input_accept.asp"
    public void InitFileUploadModal(string fileFilters = "")
    {
      modalSize = ModalSize.Default;
      ChildModalContent = new RenderFragment(builder => {
        builder.OpenComponent<UploadFileDialog>(5);
        builder.AddAttribute(7, "onSave", EventCallback.Factory.Create<object>(this, ModalCallback));
        builder.AddAttribute(8, "FileFilters", fileFilters);
        builder.CloseComponent();
      });

      StateHasChanged();
    }

    public Task OnModalClose(ModalClosingEventArgs e)
    {
      if (e.CloseReason != CloseReason.None)
      {
        OnModalSave(null);
      }
      return Task.CompletedTask;
    }

    Task ModalCallback(object response)
    {
      OnModalSave(response);
      ModalRef.Close(CloseReason.None);
      return Task.CompletedTask;
    }

    public void OpenModal()
    {      
      ModalRef.Show();
    }
    public async void OpenModalAsync()
    {
      await ModalRef.Show();
    }


    public virtual void OnModalSave(object data)
    {
    }
  }
}
