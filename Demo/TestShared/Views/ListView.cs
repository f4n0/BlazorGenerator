using Microsoft.AspNetCore.Components;
using TestShared.Data;
using static Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size16;
using BlazorEngine.Attributes;
using BlazorEngine.Layouts;
using BlazorEngine.Models;
using BlazorEngine.Utils;

namespace TestShared.Views
{
  [Route("list")]
  [AddToMenu(Title = "List Page", Route = "list", Icon = typeof(AddSquare))]
  public class ListView : ListPage<Mock>
  {
    public override string Title => "List View";
    public override Type? EditFormType => typeof(MockEditForm);
    public override bool UseVirtualization => true;
    protected override Task LoadVisibleFields()
    {
      VisibleFields = [];
      VisibleFields.AddField(nameof(Mock.Id), (ref VisibleField<Mock> prop) => prop.Href = (data) => "/test/" + data.Id);
      VisibleFields.AddField(nameof(Mock.Name)).AddFieldProperty(pro =>
      {
        pro.Get = (args) =>
        {
          return args.Data.Name;
        };
      });
      VisibleFields.AddField(nameof(Mock.Price));
      VisibleFields.AddField(nameof(Mock.Description));
      VisibleFields.AddField(nameof(Mock.OrderDate));
      VisibleFields.AddField(nameof(Mock.Icon));
      VisibleFields.AddField(nameof(Mock.Type));
      VisibleFields.AddField(nameof(Mock.Enabled));
      return Task.CompletedTask;
    }


    protected override Task LoadData()
    {
      Content = Mock.GetMultipleMock(10).OrderBy(p => p.Id);
      return Task.CompletedTask;
    }

    public override void OnSave(Mock? entity)
    {
      if(entity == null)
        return;
      var tmp = Content!.ToList();
      tmp[Content!.ToList().FindIndex(o => o.Id == entity.Id)] = entity;
      Content = tmp;
      StateHasChanged();
    }

    public void Save(Mock Rec, Mock xRec)
    {
      var tmp = Content!.ToList();
      tmp[Content!.ToList().FindIndex(o => o.Id == xRec.Id)] = Rec;
      Content = tmp;
    }
    [PageAction(Caption = "Refresh")]
    public async Task Refresh()
    {
      await LoadData();
      StateHasChanged();
    }
    
    [PageAction(Caption = "Add Rows")]
    public async Task PageAction()
    {
      var test = Content?.ToList() ?? new List<Mock>();
     test.AddRange(Mock.GetMultipleMock(10));
     Content = test;
      StateHasChanged();
    }
    
    [PageAction(Caption = "Remove Rows")]
    public async Task RemoveRows()
    {
      Content = null;
      StateHasChanged();
    }

    [GridAction(Caption = "Install", GridIcon = typeof(AirplaneTakeOff))]
    [PageAction(Caption = "Install")]
    public void GridAction(Mock? rec = null)
    {
      _ = rec;
      UIServices!.DialogService.ShowInfo("Grid Action");
    }

    [PageAction(Caption = "ShowProgress")]
    [GridAction(Caption = "ShowProgress")]
    public async Task ShowProgress()
    {
      UIServices!.ProgressService.StartProgress();
      await Task.Delay(10000);
      UIServices!.ProgressService.StopProgress();
    }

    [PageAction(Caption = "Open Modal")]
    public async Task OpenModal()
    {
      var mock = Mock.GetSingleMock();
      _ = await UIServices!.OpenModal(typeof(ModalView), mock);
    }

    [PageAction(Caption = "Go To")]
    public void GoTo()
    {
      NavManager.NavigateTo("WorksheetView", true);
    }

    [PageAction(Caption = "Test1", Group = "grouped")]
    public async Task Test1()
    {
      var mock = Mock.GetSingleMock();
      _ = await UIServices!.OpenModal(typeof(ModalView), mock);
    }
    [PageAction(Caption = "Test2", Group = "grouped")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Test Only")]
    public void Test2()
    {
    }
  }
}
