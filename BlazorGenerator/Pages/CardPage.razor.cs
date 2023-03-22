using BlazorGenerator.Models;
using System.Collections.Generic;
using BlazorGenerator.Infrastructure;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorGenerator.Pages
{
  partial class CardPage<T> : BlazorgenBaseComponent
  {
    public T Data { get; set; }

    public List<VisibleField<T>> VisibleFields { get; set; } = new List<VisibleField<T>>();
    
    public virtual void OnModify(T Data)
    {
      if (IsModal)
      {
        ModalSuccess(Data);
      }
    }

    public override void Dispose()
    {
      if (IsModal)
      {
        ModalSuccess(Data);
      }
    }


    public void Enter(KeyboardEventArgs e)
    {
      if (!IsModal) return;

      if (e.Code == "Enter" || e.Code == "NumpadEnter")
      {
        ModalSuccess(Data);
      }
    }

  }
}
