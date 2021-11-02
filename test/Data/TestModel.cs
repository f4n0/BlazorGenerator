using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Eos.Blazor.Generator;
using Microsoft.AspNetCore.Components;
using Eos.Blazor.Generator.Attributes;
using Eos.Blazor.Generator.Components;


namespace test.Data
{
  [AddToMenu("test", "/test", "oi oi-plus")]
  [Route(Route)]
  public class TestModel : CardPage
  {
    const string Route = "/test";

    [Visible("Summary")]
    public string Summary { get; set; }

    private List<dynamic> GetData()
    {
      return (new List<dynamic>(){
        new TestModel() { Summary = "element 1" },
        new TestModel() { Summary = "element 2" },
        new TestModel() { Summary = "element 3" }
      });
    }

    protected override void OnInitialized()
    {
      Data = new TestModel() { Summary = "element 1" };
    }
    

  }
}
