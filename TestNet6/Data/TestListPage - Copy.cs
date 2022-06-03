using BlazorGenerator.Attributes;
using BlazorGenerator.Components;
using BlazorGenerator.Enum;
using BlazorGenerator.Models;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TestNet6.Data
{
  [AddToMenu("List Page 2", route)]
  [Route(route)]
  [BasicActions(true, true, true)]
  public class TestListPage2 : ListPage<TestModel>
  {
    public override string Title => "My List"; 
    const string route = "/List2";



    protected override void OnInitialized()
    {
      VisibleFields = VisibleField<TestModel>.loadAllFields();

      Data = new List<TestModel>()
        {
        new TestModel(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new TestModel(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new TestModel(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new TestModel(){Test = "TestString4", Test1= 4, Test2=DateTime.Now, Test3 = 0.4M}
        };
    }

    [PageAction("Restore")]
    public void Action1()
    {
      Data.AddRange(new List<TestModel>()
        {
        new TestModel(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new TestModel(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new TestModel(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new TestModel(){Test = "TestString4", Test1= 4, Test2=DateTime.Now, Test3 = 0.4M}
        });
      Refresh();
    }
    [PageAction("Restore2")]
    public void Action4()
    {
      Data.AddRange(new List<TestModel>()
        {
        new TestModel(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new TestModel(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new TestModel(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new TestModel(){Test = "TestString4", Test1= 4, Test2=DateTime.Now, Test3 = 0.4M}
        });
      Refresh();
    }

    [PageAction("Delete all", "gruppo1")]
    public void Action2()
    {
      Data.Clear();
      Refresh();
    }

    [PageAction("Test","gruppo1")]
    public void Action3()
    {
      if (SelectedRecs.Count > 0)
      {
        var index = Data.IndexOf(SelectedRecs.First());
        var orig = Data[index];
        orig.Test = "I've Changed It";
        Data[index] = orig;
        Refresh();
      }
    }

    public override TestModel CreateNewItem()
    {
      return (new TestModel());
    }

    public override void OnModify(SavedRowItem<TestModel, Dictionary<string, object>> e)
    {
      Data[Data.IndexOf(e.Item)] = e.Item;
    }

    public override void OnInsert(SavedRowItem<TestModel, Dictionary<string, object>> e)
    {
      Data[Data.IndexOf(e.Item)] = e.Item;
    }

  }
}
