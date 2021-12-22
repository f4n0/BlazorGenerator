using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Components
{
  public partial class ModalPage<T> : ComponentBase
  {
    public virtual string Title => "";
    public virtual bool ShowSave => false;

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
