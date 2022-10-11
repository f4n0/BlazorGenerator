using System;
using System.Collections.Generic;
using BlazorGenerator;
using Microsoft.AspNetCore.Components;
using BlazorGenerator.Attributes;
using BlazorGenerator.Pages;
using BlazorGenerator.Models;
using Blazorise;
using Microsoft.AspNetCore.Components.Rendering;
using BlazorGenerator.Enum;
using BlazorGenerator.Services;

namespace TestNet6.Data
{
  public class TestNewModal : CardPage<TestNewModal>
  {
    
    public override string Title => "Card Page";
    const string Route = "/Card";

    [Parameter]
    public string test { get; set; }

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    public string? Summary { get; set; }
    public string? Summary2 { get; set; }
    public string? Summary3 { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.


    protected override void OnInitialized()
    {
      VisibleFields = new List<VisibleField<TestNewModal>>() {
        new VisibleField<TestNewModal>(nameof(test)){Getter = f => f.test, Setter = (f,v)=>f.test = v.ToString(), FullWidht=true, Editable= true}

      };
      Data = new();

    }
   

  }
}
