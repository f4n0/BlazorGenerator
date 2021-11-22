using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Eos.Blazor.Generator;
using Microsoft.AspNetCore.Components;
using Eos.Blazor.Generator.Attributes;
using Eos.Blazor.Generator.Components;
using Eos.Blazor.Generator.Models;


namespace test.Data
{
  

  [AddToMenu("test", "/test", "oi oi-plus")]
  [Route(Route)]
  public class TestModel : CardPage<TestModel>
  {
    const string Route = "/test";
    [Inject] NavigationManager Manager { get; set; }

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    public string? Summary { get; set; }
    public string? Summary2 { get; set; }
    public string? Summary3 { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.


    protected override void OnInitialized()
    {
      VisibleFields = new List<VisibleField<TestModel>>() {
        new VisibleField<TestModel>(nameof(Summary)){Getter = f => f.Summary, Setter = (f,v)=>f.Summary = v.ToString()},
        new VisibleField<TestModel>(nameof(Summary2)){Getter = f => f.Summary2, Setter = (f,v)=>f.Summary2 = v.ToString()},
        new VisibleField<TestModel>(nameof(Summary3)){Getter = f => f.Summary3, Setter = (f,v)=>f.Summary3 = v.ToString()}
      };
      Data = new TestModel() { Summary = "element 1", Summary2 = "element2", Summary3 = "aa" };
    }

    [PageAction]
    public void Test()
    {
      Manager.NavigateTo("/test/prova");

    }

  }
}
