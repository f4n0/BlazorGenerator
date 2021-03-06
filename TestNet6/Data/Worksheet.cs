using BlazorGenerator.Attributes;
using BlazorGenerator.Components;
using BlazorGenerator.Enum;
using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TestNet6.Data
{
  [AddToMenu(Title = "Worksheet Page", Route = route, OrderSequence = 1)]
  [Route(route)]
  [BasicActions(true, true, true)]
  public class ComplexExample2 : WorksheetPage<TestCardPage, TestListPage>
  {
    public override string Title => "My Worksheet Page"; 
    const string route = "/Worksheet";

    public string Test { get; set; }
    public bool Test6 { get; set; }
    public int Test1 { get; set; }
    public DateTime Test2 { get; set; }
    public decimal Test3 { get; set; }
    public Uri? Test4 { get; set; }
    public FieldType? Test5 { get; set; }

    protected override void OnInitialized()
    {
      setLogVisibility(true);


      VisibleFields = new List<VisibleField<TestCardPage>>() {
        new VisibleField<TestCardPage>(nameof(TestCardPage.Summary)){Getter = f => f.Summary, Setter = (f,v)=>f.Summary = v.ToString(), FullWidht=true},
        new VisibleField<TestCardPage>(nameof(TestCardPage.Summary2)){Getter = f => f.Summary2, Setter = (f,v)=>f.Summary2 = v.ToString(), TextRole = Blazorise.TextRole.Password},
        new VisibleField<TestCardPage>(nameof(TestCardPage.Summary3)){Getter = f => f.Summary3, Setter = (f,v)=>f.Summary3 = v.ToString()},

      new VisibleField<TestCardPage>("My BTN", FieldType.Button){ Setter = (f,v) => throw new Exception("prova") }
      };
      Data = new TestCardPage() { Summary = "element 1", Summary2 = "element2", Summary3 = "aa" };


      ListVisibleFields = new List<VisibleField<TestListPage>>()        {
      new VisibleField<TestListPage>(nameof(Test)){ Getter = f => f.Test, Setter = (f, v) =>  f.Test = v as string},
      new VisibleField<TestListPage>(nameof(Test6), FieldType.Boolean){Getter = f => f.Test6, Setter = (f,v) => f.Test6 = (bool)v },
      new VisibleField<TestListPage>(nameof(Test1)){ Getter = f => f.Test1.ToString(), Setter = (f, v) =>  f.Test1 = int.Parse(v as string)},
      new VisibleField<TestListPage>(nameof(Test2)){ Getter = f => f.Test2.ToString(), Setter = (f, v) =>  f.Test2 = DateTime.Parse(v as string)},
      new VisibleField<TestListPage>(nameof(Test3)){ Getter = f => f.Test3.ToString(), Setter = (f, v) =>  f.Test3 = decimal.Parse(v as string)},
      new VisibleField<TestListPage>(nameof(Test4), FieldType.Custom, true)
      { EditOnly=true,
        Caption="Test 4",
        Getter = f => f.Test4,
        Setter = (f, v) =>  {
          var builder = new UriBuilder(v.ToString());
          f.Test4 = builder.Uri;
        }
      },
      new VisibleField<TestListPage>(nameof(Test5), FieldType.Select){
        Getter = f => f.Test5.GetValueOrDefault(),
        Setter = (f, v) => f.Test5 = (FieldType)Enum.Parse(typeof(FieldType), v.ToString()),
        Values = Enum.GetNames(typeof(FieldType))
      },
      new VisibleField<TestListPage>("Icon", FieldType.Icon){
        Getter = f => Blazorise.IconName.Archive
      }
      };

      ListData = new List<TestListPage>()
        {
        new TestListPage(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new TestListPage(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new TestListPage(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new TestListPage(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new TestListPage(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new TestListPage(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new TestListPage(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new TestListPage(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new TestListPage(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new TestListPage(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new TestListPage(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new TestListPage(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new TestListPage(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new TestListPage(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new TestListPage(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new TestListPage(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new TestListPage(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new TestListPage(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new TestListPage(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new TestListPage(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new TestListPage(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new TestListPage(){Test = "TestString4", Test1= 4, Test2=DateTime.Now, Test3 = 0.4M}
        };
    }

    [PageAction("Restore")]
    public void Action1()
    {
      ListData.AddRange(new List<TestListPage>()
        {
        new TestListPage(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new TestListPage(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new TestListPage(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new TestListPage(){Test = "TestString4", Test1= 4, Test2=DateTime.Now, Test3 = 0.4M}
        });
      Refresh();
    }
    [PageAction("Restore2")]
    public void Action4()
    {
      ListData.AddRange(new List<TestListPage>()
        {
        new TestListPage(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new TestListPage(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new TestListPage(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new TestListPage(){Test = "TestString4", Test1= 4, Test2=DateTime.Now, Test3 = 0.4M}
        });
      Refresh();
    }

    [PageAction("Delete all", "gruppo1")]
    public void Action2()
    {
      ListData.Clear();
      Refresh();
    }

    [PageAction("Test","gruppo1")]
    public void Action3()
    {
      if (SelectedRecs.Count > 0)
      {
        var index = ListData.IndexOf(SelectedRecs.First());
        var orig = ListData[index];
        orig.Test = "I've Changed It";
        ListData[index] = orig;
        Refresh();
      }
    }

    public override TestListPage CreateNewItem()
    {
      return (new TestListPage());
    }

  }
}
