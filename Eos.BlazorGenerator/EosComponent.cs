using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Components;

namespace Eos.BlazorGenerator
{
  [Layout(typeof(EosComponent))]

  public class EosComponent : LayoutComponentBase
  {
    protected EosComponent() { }

    [Parameter]
    public RenderFragment? Body { get; set; }

    public object? Data;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
      builder.OpenElement(0, "h1");
      builder.AddContent(1,"AHAHAHA");
      builder.AddContent(2, Body);
      builder.CloseComponent();
      base.BuildRenderTree(builder);
    }
  }
}
