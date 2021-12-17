using Blazorise;
using Blazorise.DataGrid;
using Eos.Blazor.Generator.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eos.Blazor.Generator.Components
{
  partial class WorksheetPage<T> : ComponentBase
  {
    [Inject] public IPageProgressService PageProgressService { get; set; }
    [Inject] public IMessageService MessageService { get; set; }

    public List<T> SelectedRecs { get; private set; } = new List<T>();
    public List<T> ListData { get; set; }
    public List<VisibleField<T>> ListVisibleFields { get; set; } = new List<VisibleField<T>>();
    private DataGrid<T> _datagrid;
    public virtual string Title => "";

    public T Data { get; set; }
    public List<VisibleField<T>> VisibleFields { get; set; } = new List<VisibleField<T>>();


    public void StartLoader()
    {
      PageProgressService.Go(null, options => { options.Color = Color.Danger; });
    }
    public void StopLoader()
    {
      PageProgressService.Go(-1);
    }

    public void Refresh()
    {
      _datagrid.Reload();
    }

    public virtual T CreateNewItem()
    {
      return Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile().Invoke();
    }


    public virtual void OnInsert(SavedRowItem<T, Dictionary<string, object>> e)
    {
    }

    public virtual void OnModify(SavedRowItem<T, Dictionary<string, object>> e)
    {
    }

    public virtual void OnDelete(T model)
    {
    }

    internal DataGridEditMode GetEditMode()
    {
      if (ListVisibleFields.Any(o => o.EditOnly))
      {
        return DataGridEditMode.Form;
      }
      else
      {
        return DataGridEditMode.Inline;
      }
    }

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

  }
}