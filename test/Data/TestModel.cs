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

    public string Summary { get; set; }
    public string Summary2 { get; set; }
    public string? Summary3 { get; set; }


    protected override void OnInitialized()
    {
      VisibleFields = new List<VisibleField<TestModel>>() {
        new VisibleField<TestModel>(nameof(Summary)){Getter = f => f.Summary, Setter = (f,v)=>f.Summary = v.ToString()},
        new VisibleField<TestModel>(nameof(Summary2)){Getter = f => f.Summary2, Setter = (f,v)=>f.Summary2 = v.ToString()},
        new VisibleField<TestModel>(nameof(Summary3)){Getter = f => f.Summary3, Setter = (f,v)=>f.Summary3 = v.ToString()}
      };
      Data = new TestModel() { Summary = "element 1", Summary2 = "element2", Summary3 = "aa" };
    }


  }
}
