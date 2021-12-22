using BlazorGenerator.Attributes;
using BlazorGenerator.Components;
using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.Data
{
  [AddToMenu("Extreme Example", route)]
  [Route(route)]
  public class ExtremeExample : CardPage<ExtremeExample>
  {
    const string route = "/ExtremeExample";

    public string Test { get; set; }
    public int Test1 { get; set; }
    public DateTime Test2 { get; set; }
    public decimal Test3 { get; set; }

    [Subform]
    public ComplexExample Complex { get; set; }

    protected override void OnInitialized()
    {
      VisibleFields = new List<VisibleField<ExtremeExample>>(){
      new VisibleField<ExtremeExample>(nameof(Test)){Getter = f => f.Test, Setter = (f, v) =>  f.Test = v as string},
      new VisibleField<ExtremeExample>(nameof(Test1)){Getter = f => f.Test1.ToString(), Setter = (f, v) =>  f.Test1 = Int32.Parse(v as string)},
      new VisibleField<ExtremeExample>(nameof(Test2)){Getter = f => f.Test2.ToString(), Setter = (f, v) =>  f.Test2 = DateTime.Parse(v as string)},
      new VisibleField<ExtremeExample>(nameof(Test3)){Getter = f => f.Test3.ToString(), Setter = (f, v) =>  f.Test3 = decimal.Parse(v as string)}
      };


      Data = new ExtremeExample() { Test = "TestString1", Test1 = 1, Test2 = DateTime.Now, Test3 = 0.1M };
    }


  }
}
