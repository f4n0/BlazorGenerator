using BlazorGenerator.Attributes;
using BlazorGenerator.Layouts;
using BlazorGenerator.Models;
using BlazorGenerator.Utils;
using Microsoft.AspNetCore.Components;
using TestShared.Data;
using static Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size16;

namespace TestShared.Views
{
  [Route("list")]
  [AddToMenu(Title = "List Page", Route = "list", Icon = typeof(AddSquare))]
  public class ListView : ListPage<Mock>
  {
    public override string Title => "List View";
    public override Type? EditFormType => typeof(MockEditForm);

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

    public override void OnSave(Mock entity)
    {
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

    [PageAction(Caption = "Page Action")]
    public async void PageAction()
    {
      var test = Selected;
      //await LoadData();
      StateHasChanged();
      //UIServices!.DialogService.ShowInfo("Page Action");
      await Task.CompletedTask;
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
    public async void ShowProgress()
    {
      UIServices!.ProgressService.StartProgress();
      await Task.Delay(10000);
      UIServices!.ProgressService.StopProgress();
    }

    [PageAction(Caption = "Open Modal")]
    public async void OpenModal()
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
    public async void Test1()
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
