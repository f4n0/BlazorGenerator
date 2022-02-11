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

    public void InitModal<TModalType, TModalData>(object ModalData) where TModalType : ModalPage<TModalData>
    {
      ModalRef.ChildContent = new RenderFragment(builder =>
      {
        builder.OpenComponent<ModalContent>(0);
        builder.AddAttribute(1, "Centered", true);
        builder.AddAttribute(1, "Size", ModalSize.ExtraLarge);
        builder.AddAttribute(2, "ChildContent", (RenderFragment)((builder2) =>
        {
          builder2.OpenComponent(3, typeof(ModalHeader));
          builder2.AddAttribute(4, "ChildContent", (RenderFragment)((builder3) =>
          {
            builder3.OpenComponent(5, typeof(CloseButton));
            builder3.CloseComponent();
          }));
          builder2.CloseComponent();

          builder2.OpenComponent<ModalBody>(4);
          builder2.AddAttribute(4, "ChildContent", (RenderFragment)((builder3) =>
          {
            builder3.OpenComponent<TModalType>(5);
            builder3.AddAttribute(6, "Data", ModalData);
            builder3.AddAttribute(7, "onSave", EventCallback.Factory.Create<object>(this, ModalCallback));
            builder3.CloseComponent();
          }));
          builder2.CloseComponent();
        }));

        builder.CloseComponent();
      });

      StateHasChanged();
    }
    Task ModalCallback(object response)
    {
      OnModalSave(response);
      ModalRef.Close(CloseReason.UserClosing);
      return Task.CompletedTask;
    }

    public void OpenModal()
    {      
      ModalRef.Show().Wait();
    }

    public virtual void OnModalSave(object data)
    {
    }
  }
}
