using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Layouts.Partial
{
  public partial class CardFields<T>
  {
    [Parameter]
    public List<VisibleField<T>> VisibleFields { get; set; } = [];

    [Parameter]
    public int GridSize { get; set; }

    [Parameter]
    public T? Data { get; set; }

    [Parameter]
    public bool ShowButtons { get; set; }

    [Parameter]
    public PermissionSet? PermissionSet { get; set; }

    [Parameter]
    public Action<T>? OnSave { get; set; }

    protected void HandleSave(T Data)
    {
      OnSave?.Invoke(Data);
    }

    [Parameter]
    public Action<T>? OnDiscard { get; set; }

    protected void HandleDiscard(T Data)
    {
      OnDiscard?.Invoke(Data);
    }
  }
}
