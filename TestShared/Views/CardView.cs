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
    protected override Task LoadVisibleFields()
    {
      VisibleFields = [];
      VisibleFields.AddField(nameof(Mock.Id));

      VisibleFields.AddField(nameof(Mock.Name)).AddFieldProperty(t => t.OnLookup = (_) => [.. (new List<string>() { "test", "test2" })]);
      VisibleFields.AddField(nameof(Mock.Price));
      VisibleFields.AddField(nameof(Mock.Description), (ref VisibleField<Mock> o) => o.TextFieldType = Microsoft.FluentUI.AspNetCore.Components.TextFieldType.Password);
      VisibleFields.AddField(nameof(Mock.OrderDate));
      VisibleFields.AddField(nameof(Mock.Type));
      VisibleFields.AddField(nameof(Mock.NullTest));

      return Task.CompletedTask;
    }

    protected override Task LoadData()
    {
      Content = Mock.GetSingleMock();
      return Task.CompletedTask;
    }

    [PageAction(Caption = "ShowProgress")]
    public async void ShowProgress()
    {
      UIServices!.ProgressService.StartProgress();
      await Task.Delay(100000);
      UIServices!.ProgressService.StopProgress();
    }

    [PageAction(Caption = "Open Modal")]
    public async void OpenModal()
    {
      var mock = Mock.GetSingleMock();
      _ = await UIServices!.OpenModal(typeof(ModalView), mock);
    }

    [PageAction(Caption = "Ask User")]
    public async void AskUser()
    {
      _ = await UIServices!.UserInput(new UserInputData()
      {
        Message = "Gimme the data"
      });

      _ = await UIServices!.UserInput(new UserInputData()
      {
        Message = "Gimme the Password",
        InputType = UserInputType.Secret
      });

      _ = await UIServices!.UserInput(new UserInputData()
      {
        Message = "Choose!",
        InputType = UserInputType.Choice,
        Choices = ["Uno", "Due", "Tre"]
      });
    }

    [PageAction(Caption = "Show Upload")]
    public async void UploadFiles()
    {
      var test = await UIServices.UploadFile();
      if (test?.Files != null)
      {
        foreach (var file in test.Files)
        {
          _ = file;
        }
      }
    }

    [PageAction(Caption = "Go To")]
    public void GoTo()
    {
      NavManager.NavigateTo("WorksheetView", true);
    }

    [PageAction(Caption = "Test1", Group = "grouped")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Test Only")]
    public void Test1()
    {

    }
    [PageAction(Caption = "Test2", Group = "grouped")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Test Only")]
    public void Test2()
    {
    }

    [PageAction(Caption = "Thorow Error")]
    public void Test3()
    {
      throw new NotImplementedException();
    }
    [PageAction(Caption = "Lock UI")]
    public async void Test4()
    {
      UIServices.LockUI();
      await Task.Delay(2000);
      UIServices.UnlockUI();
    }

    [PageAction(Caption = "Write Log")]
    public void Test5()
    {
      for (var i = 0; i < 30; i++)
      {
        UIServices.Logger.SendLogMessage($"Test{i}");
      }
    }
    [PageAction(Caption = "Test6")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Test Only")]
    public void Test6()
    {
    }
  }
}
