using Eos.Blazor.Generator.Attributes;
using Eos.Blazor.Generator.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.Data
{
  [AddToMenu("Extreme Example", route)]
  [Route(route)]
  public class ExtremeExample : CardPage
  {
    const string route = "/ExtremeExample";

    [Visible]
    public string Test { get; set; }
    [Visible]
    public int Test1 { get; set; }
    [Visible]
    public DateTime Test2 { get; set; }
    [Visible]
    public decimal Test3 { get; set; }

    [Subform]
    public ComplexExample Complex { get; set; }

    protected override void OnInitialized()
    {
      Data = new ExtremeExample() { Test = "TestString1", Test1 = 1, Test2 = DateTime.Now, Test3 = 0.1M };
    }


  }
}
