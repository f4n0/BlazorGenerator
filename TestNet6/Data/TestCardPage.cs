using System;
using System.Collections.Generic;
using BlazorGenerator;
using Microsoft.AspNetCore.Components;
using BlazorGenerator.Attributes;
using BlazorGenerator.Components;
using BlazorGenerator.Models;
using Blazorise;
using Microsoft.AspNetCore.Components.Rendering;
using BlazorGenerator.Enum;
using BlazorGenerator.Services;

namespace TestNet6.Data
{


  [AddToMenu(Title = "Card Page", Route = Route, Icon = IconName.Tag, OrderSequence = 5)]
  [Route(Route)]
  public class TestCardPage : CardPage<TestCardPage>
  {
    
    public override string Title => "Card Page";
    const string Route = "/Card";

    [Inject] public BlazorGenLogger blazorGenLogger { get; set; }

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    public string? Summary { get; set; }
    public string? Summary2 { get; set; }
    public string? Summary3 { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.


    protected override void OnInitialized()
    {
      setLogVisibility(true);

      VisibleFields = new List<VisibleField<TestCardPage>>() {
        new VisibleField<TestCardPage>(nameof(Summary)){Getter = f => f.Summary, Setter = (f,v)=>f.Summary = v.ToString(), FullWidht=true},
        new VisibleField<TestCardPage>(nameof(Summary2)){Getter = f => f.Summary2, Setter = (f,v)=>f.Summary2 = v.ToString(), TextRole = TextRole.Password},
        new VisibleField<TestCardPage>(nameof(Summary3)){Getter = f => f.Summary3, Setter = (f,v)=>f.Summary3 = v.ToString()},

      new VisibleField<TestCardPage>("My BTN", FieldType.Button){ Setter = (f,v) => throw new Exception("prova") }
      };
      Data = new TestCardPage() { Summary = "element 1", Summary2 = "element2", Summary3 = "aa" };

    }

    [PageAction]
    public void Test()
    {
      NavManager.NavigateTo("/test/prova");
    }


    [PageAction]
    public async Task ChoseAsync()
    {
      var res = await ShowChoose("Title", new[] { "Opzione 1", "Opzione 2", "Opzione 3" });
      MessageService.Success(res);
    }

    [PageAction]
    public void ShowModal()
    {
      var tmp = new MyModalContent() { Summary = "element aa", Summary2 = "elementbb", Summary3 = "cc" };
      InitModal<MyModalContent, MyModalContent>(tmp);
      OpenModal();

    }


    [PageAction]
    public void ShowUpload()
    {
      InitFileUploadModal(".txt"); 
      OpenModal();
    }

    public override void OnModalSave(object data)
    {
      if (data == null)
        return;

      if(data.GetType() == typeof(byte[]))
      {
        var content = System.Text.Encoding.Default.GetString((byte[])data);
         MessageService.Success(content);
      }
    }


    [PageAction]
    public void AddLog()
    {
      blazorGenLogger.SendLogMessage("test");
    }

  }
}
