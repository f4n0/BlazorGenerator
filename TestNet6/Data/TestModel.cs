using System;
using System.Collections.Generic;
using BlazorGenerator;
using Microsoft.AspNetCore.Components;
using BlazorGenerator.Attributes;
using BlazorGenerator.Components;
using BlazorGenerator.Models;
using Blazorise;
using Microsoft.AspNetCore.Components.Rendering;
using BlazorGenerator.Enum;
using BlazorGenerator.Services;

namespace TestNet6.Data
{


  [AddToMenu("test", "/test", IconName.Tag)]
  [Route(Route)]
  public class TestModel : CardPage<TestModel>
  {
    
    public override string Title => "Test Model";
    const string Route = "/test";

    [Inject] public BlazorGenLogger blazorGenLogger { get; set; }

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    public string? Summary { get; set; }
    public string? Summary2 { get; set; }
    public string? Summary3 { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.


    protected override void OnInitialized()
    {
      setLogVisibility(true);

      VisibleFields = new List<VisibleField<TestModel>>() {
        new VisibleField<TestModel>(nameof(Summary)){Getter = f => f.Summary, Setter = (f,v)=>f.Summary = v.ToString()},
        new VisibleField<TestModel>(nameof(Summary2)){Getter = f => f.Summary2, Setter = (f,v)=>f.Summary2 = v.ToString(), TextRole = TextRole.Password},
        new VisibleField<TestModel>(nameof(Summary3)){Getter = f => f.Summary3, Setter = (f,v)=>f.Summary3 = v.ToString()},

      new VisibleField<TestModel>("My BTN", FieldType.Button){ Setter = (f,v) => throw new Exception("prova") }
      };
      Data = new TestModel() { Summary = "element 1", Summary2 = "element2", Summary3 = "aa" };

    }

    [PageAction]
    public void Test()
    {
      NavManager.NavigateTo("/test/prova");
    }

    [PageAction]
    public void ShowModal()
    {
      var tmp = new MyModalContent() { Summary = "element aa", Summary2 = "elementbb", Summary3 = "cc" };
      InitModal<MyModalContent, MyModalContent>(tmp);
      OpenModal();

    }

    [PageAction]
    public void AddLog()
    {
      blazorGenLogger.SendLogMessage("test");
    }

  }
}
