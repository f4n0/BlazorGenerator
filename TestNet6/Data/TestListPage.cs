using BlazorGenerator.Attributes;
using BlazorGenerator.Pages;
using BlazorGenerator.Enum;
using BlazorGenerator.Models;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using BlazorGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blazorise.Icons.FontAwesome;
using DocumentFormat.OpenXml.Wordprocessing;

namespace TestNet6.Data
{
  [AddToMenu(Title = "List Page", Route = route, OrderSequence = 3, Icon = BlazorGenIcons.Adjust)]
  [Route(route)]
  public class TestListPage : ListPage<TestListPage>
  {
    public override string Title => "My List";
    const string route = "/List";

    public override bool showGrouping => true;

    public string Test { get; set; }
    public bool Test6 { get; set; }
    public int Test1 { get; set; }
    public DateTime Test2 { get; set; }
    public decimal Test3 { get; set; }
    public Uri? Test4 { get; set; }
    public FieldType? Test5 { get; set; }

    public override bool GroupByEnabled() => true;


    public override Action<TestListPage, DataGridRowStyling> RowStyling => (row, style) =>
    {
      if ((row as TestListPage).Test == "TestString1")
      {
        style.Background = Blazorise.Background.Success;
      }
    };

    protected override void OnInitialized()
    {

      setLogVisibility(true);

      VisibleFields = new List<VisibleField<TestListPage>>()        {
      new VisibleField<TestListPage>(nameof(Test)){ Getter = f => f.Test, Setter = (f, v) =>  f.Test = v as string, Groupable=true},
      new VisibleField<TestListPage>(nameof(Test1)){ Getter = f => f.Test1.ToString(), Setter = (f, v) =>  f.Test1 = int.Parse(v as string), Groupable=true},
      new VisibleField<TestListPage>(nameof(Test6), FieldType.Boolean){Getter = f => f.Test6, Setter = (f,v) => f.Test6 = (bool)v , Groupable=true},
      new VisibleField<TestListPage>(nameof(Test2)){ Getter = f => f.Test2.ToString(), Setter = (f, v) =>  f.Test2 = DateTime.Parse(v as string), Groupable=true},
      new VisibleField<TestListPage>(nameof(Test3)){ Getter = f => f.Test3.ToString(), Setter = (f, v) =>  f.Test3 = decimal.Parse(v as string), Groupable=true},
      new VisibleField<TestListPage>(nameof(Test4), FieldType.Custom, true)
      {
        EditOnly=true,
        Caption="Test 4",
        Getter = f => f.Test4,
        Setter = (f, v) =>  {
          var builder = new UriBuilder(v.ToString());
          f.Test4 = builder.Uri;
        }
      },
      new VisibleField<TestListPage>(nameof(Test5), FieldType.Select){
        Editable = true,
        Getter = f => f.Test5.GetValueOrDefault(),
        Setter = (f, v) => f.Test5 = (FieldType)Enum.Parse(typeof(FieldType), v.ToString()),
        Values = Enum.GetNames(typeof(FieldType))
      },
      new VisibleField<TestListPage>("Test btn", FieldType.Button,false)
      };

      Data = new List<TestListPage>()
        {
        new TestListPage(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new TestListPage(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new TestListPage(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new TestListPage(){Test = "TestString4", Test1= 4, Test2=DateTime.Now, Test3 = 0.4M}
        };
    }

    [PageAction(Caption = "Restore", Icon =  BlazorGenIcons.Adjust)]
    public void Action1()
    {
      Data.AddRange(new List<TestListPage>()
        {
        new TestListPage(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new TestListPage(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new TestListPage(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new TestListPage(){Test = "TestString4", Test1= 4, Test2=DateTime.Now, Test3 = 0.4M}
        });
      Refresh();
    }
    [PageAction(Caption = "Restore2")]
    [ContextMenu]
    public void Action4()
    {
      Data.AddRange(new List<TestListPage>()
        {
        new TestListPage(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new TestListPage(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new TestListPage(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new TestListPage(){Test = "TestString4", Test1= 4, Test2=DateTime.Now, Test3 = 0.4M}
        });
      Refresh();
    }

    [PageAction(Caption = "Delete all", Group = "gruppo1")]
    public void Action2()
    {
      Data.Clear();
      Refresh();
    }

    [PageAction(Caption = "Test", Group = "gruppo1")]
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
    [PageAction(Caption = "Delete all 2", Group = "gruppo1")]
    public void Action22()
    {
      Data.Clear();
      Refresh();
    }

    [PageAction(Caption = "Test2", Group = "gruppo1")]
    public void Action32()
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


    public override TestListPage CreateNewItem()
    {
      return (new TestListPage());
    }

    public override void OnModify(SavedRowItem<TestListPage, Dictionary<string, object>> e)
    {
      Data[Data.IndexOf(e.Item)] = e.Item;
    }

    public override void OnInsert(SavedRowItem<TestListPage, Dictionary<string, object>> e)
    {
      Data[Data.IndexOf(e.Item)] = e.Item;
    }

  }
}
