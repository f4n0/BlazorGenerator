﻿using BlazorGenerator.Attributes;
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
  [AddToMenu("Complex2", route)]
  [Route(route)]
  [BasicActions(true, true, true)]
  public class ComplexExample2 : WorksheetPage<TestModel, ComplexExample>
  {
    public override string Title => "Is that a list?"; 
    const string route = "/complex2";

    public string Test { get; set; }
    public bool Test6 { get; set; }
    public int Test1 { get; set; }
    public DateTime Test2 { get; set; }
    public decimal Test3 { get; set; }
    public Uri? Test4 { get; set; }
    public FieldType? Test5 { get; set; }

    protected override void OnInitialized()
    {
      ListVisibleFields = new List<VisibleField<ComplexExample>>()        {
      new VisibleField<ComplexExample>(nameof(Test)){ Getter = f => f.Test, Setter = (f, v) =>  f.Test = v as string},
      new VisibleField<ComplexExample>(nameof(Test6), FieldType.Boolean){Getter = f => f.Test6, Setter = (f,v) => f.Test6 = (bool)v },
      new VisibleField<ComplexExample>(nameof(Test1)){ Getter = f => f.Test1.ToString(), Setter = (f, v) =>  f.Test1 = int.Parse(v as string)},
      new VisibleField<ComplexExample>(nameof(Test2)){ Getter = f => f.Test2.ToString(), Setter = (f, v) =>  f.Test2 = DateTime.Parse(v as string)},
      new VisibleField<ComplexExample>(nameof(Test3)){ Getter = f => f.Test3.ToString(), Setter = (f, v) =>  f.Test3 = decimal.Parse(v as string)},
      new VisibleField<ComplexExample>(nameof(Test4), FieldType.Custom, true)
      { EditOnly=true,
        Caption="Test 4",
        Getter = f => f.Test4,
        Setter = (f, v) =>  {
          var builder = new UriBuilder(v.ToString());
          f.Test4 = builder.Uri;
        }
      },
      new VisibleField<ComplexExample>(nameof(Test5), FieldType.Select){
        Getter = f => f.Test5.GetValueOrDefault(),
        Setter = (f, v) => f.Test5 = (FieldType)Enum.Parse(typeof(FieldType), v.ToString()),
        Values = Enum.GetNames(typeof(FieldType))
      }
      };

      ListData = new List<ComplexExample>()
        {
        new ComplexExample(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new ComplexExample(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new ComplexExample(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new ComplexExample(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new ComplexExample(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new ComplexExample(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new ComplexExample(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new ComplexExample(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new ComplexExample(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new ComplexExample(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new ComplexExample(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new ComplexExample(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new ComplexExample(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new ComplexExample(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new ComplexExample(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new ComplexExample(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new ComplexExample(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new ComplexExample(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new ComplexExample(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new ComplexExample(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new ComplexExample(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new ComplexExample(){Test = "TestString4", Test1= 4, Test2=DateTime.Now, Test3 = 0.4M}
        };
    }

    [PageAction("Restore")]
    public void Action1()
    {
      ListData.AddRange(new List<ComplexExample>()
        {
        new ComplexExample(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new ComplexExample(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new ComplexExample(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new ComplexExample(){Test = "TestString4", Test1= 4, Test2=DateTime.Now, Test3 = 0.4M}
        });
      Refresh();
    }
    [PageAction("Restore2")]
    public void Action4()
    {
      ListData.AddRange(new List<ComplexExample>()
        {
        new ComplexExample(){Test = "TestString1", Test1= 1, Test2=DateTime.Now, Test3 = 0.1M},
        new ComplexExample(){Test = "TestString2", Test1= 2, Test2=DateTime.Now, Test3 = 0.2M},
        new ComplexExample(){Test = "TestString3", Test1= 3, Test2=DateTime.Now, Test3 = 0.3M},
        new ComplexExample(){Test = "TestString4", Test1= 4, Test2=DateTime.Now, Test3 = 0.4M}
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

    public override ComplexExample CreateNewItem()
    {
      return (new ComplexExample());
    }

  }
}