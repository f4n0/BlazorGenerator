using BlazorGenerator.Attributes;
using BlazorGenerator.Layouts;
using BlazorGenerator.Models;
using BlazorGenerator.Utils;
using Microsoft.AspNetCore.Components;
using TestShared.Data;

namespace TestShared.Views
{
  [Route("card")]
  [AddToMenu(Title = "Card Page", Route = "card")]
  public class CardView : CardPage<Mock>
  {

    protected override async Task LoadVisibleFields()
    {
      VisibleFields = [];
      VisibleFields.AddField(nameof(Mock.Id));
      VisibleFields.AddField(nameof(Mock.Name), (ref VisibleField < Mock > o) =>
      {
        o.OnLookup = (data) => (new List<string>() { "test", "test2" }).ToList<object>();
      });
      VisibleFields.AddField(nameof(Mock.Price));
      VisibleFields.AddField(nameof(Mock.Description), (ref VisibleField<Mock> o) => o.TextFieldType = Microsoft.FluentUI.AspNetCore.Components.TextFieldType.Password);
      VisibleFields.AddField(nameof(Mock.OrderDate));
      VisibleFields.AddField(nameof(Mock.type));
      VisibleFields.AddField(nameof(Mock.NullTest));


    }

    protected override async Task LoadData()
    {
      Content = Mock.getSingleMock();
    }

    [PageAction(Caption = "ShowProgress")]
    public async void ShowProgress()
    {
      UIServices!.ProgressService.StartProgress();
      await Task.Delay(10000);
      UIServices!.ProgressService.StopProgress();
    }

    [PageAction(Caption = "Open Modal")]
    public async void OpenModal()
    {
      var mock = Mock.getSingleMock();
      _ = await UIServices!.OpenModal(typeof(ModalView), mock);
    }

    [PageAction(Caption = "Go To")]
    public async void GoTo()
    {
      NavManager.NavigateTo("WorksheetView", true);
    }


    [PageAction(Caption = "Test1", Group = "grouped")]
    public void Test1()
    {
    }
    [PageAction(Caption = "Test2", Group = "grouped")]
    public void Test2()
    {
    }
  }
}
