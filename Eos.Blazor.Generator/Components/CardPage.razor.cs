using Blazorise;
using Eos.Blazor.Generator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eos.Blazor.Generator.Components
{
  partial class CardPage<T> : ComponentBase
  {
    public T Data { get; set; }
    public virtual string Title => "";

    public List<VisibleField<T>> VisibleFields { get; set; } = new List<VisibleField<T>>();
    Modal ModalRef;

    public void InitModal<TModalType, TModalData>(object ModalData) where TModalType : ModalPage<TModalData>
    {
      ModalRef.ChildContent = new RenderFragment(builder =>
      {
        builder.OpenComponent<Blazorise.ModalContent>(0);
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

          builder2.OpenComponent<Blazorise.ModalBody>(4);
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
    }
    Task ModalCallback(object response)
    {
      OnModalSave(response);
      ModalRef.Close(CloseReason.UserClosing);
      return Task.CompletedTask;
    }

    public void OpenModal()
    {
      ModalRef.Show();
    }

    public virtual void OnModalSave(object data)
    {
    }

    public virtual void OnModify(T Data)
    {
    }

  }
}
