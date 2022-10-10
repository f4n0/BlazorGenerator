using Blazorise;
using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using BlazorGenerator.Infrastructure;

namespace BlazorGenerator.Pages
{
  partial class CardPage<T> : BlazorgenBaseComponent
  {
    public T Data { get; set; }

    public List<VisibleField<T>> VisibleFields { get; set; } = new List<VisibleField<T>>();
    
    public virtual void OnModify(T Data)
    {
    }

    public override void Dispose()
    {
      if (IsModal)
      {
        ModalSuccess(Data);
      }
    }

  }
}
