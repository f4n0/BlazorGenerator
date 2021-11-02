using Eos.Blazor.Generator.Attributes;
using Eos.Blazor.Generator.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test.Data
{
  [AddToMenu("Complex", route)]
  [Route(route)]
  public class ComplexExample : ListPage
  {
    const string route = "/complex";

    [Visible]
    public string Test { get; set; }
    [Visible]
    public int Test1 { get; set; }
    [Visible]
    public DateTime Test2 { get; set; }
    [Visible]
    public decimal Test3 { get; set; }

    protected override void OnInitialized()
    {
      Data = new List<dynamic>()
      {
        new ComplexExample(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new ComplexExample(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new ComplexExample(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new ComplexExample(){Test = "TestString4", Test1= 4, Test2=DateTime.Now, Test3 = 0.4M}
      };
    }

    [PageAction("Restore")]
    public void Action1()
    {
      Data.AddRange( new List<dynamic>()
      {
        new ComplexExample(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new ComplexExample(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new ComplexExample(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new ComplexExample(){Test = "TestString4", Test1= 4, Test2=DateTime.Now, Test3 = 0.4M}
      });
    }

    [PageAction("Delete all")]
    public void Action2()
    {
      Data.Clear();
    }
  }
}
