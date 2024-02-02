using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Layouts.Partial
{
  internal class ListEditPanel<T> : CardPage<T>
  {
    [Parameter]
   public new ModalData<T> Content { get; set; } = null!;

    protected override Task OnParametersSetAsync()
    {
      //Data = Content.Data;
      VisibleFields = Content.VisibleFields;
      ShowActions = false;

      return base.OnParametersSetAsync();
    }

    public override bool ShowButtons => false;
  }
}
