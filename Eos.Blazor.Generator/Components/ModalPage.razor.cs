using Eos.Blazor.Generator.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eos.Blazor.Generator.Components
{
  public partial class ModalPage<T> : ComponentBase
  {
    private T _data;
    [Parameter]
    public T Data
    {
      get { return _data; }
      set
      {
        _data = value;
      }
    }
    public List<VisibleField<T>> VisibleFields { get; set; } = new List<VisibleField<T>>();

    [Parameter] public EventCallback<object> onSave { get; set; }
  }
}
