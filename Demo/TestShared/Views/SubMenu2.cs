using Microsoft.AspNetCore.Components;
using TestShared.Data;
using BlazorEngine.Attributes;
using BlazorEngine.Layouts;
using BlazorEngine.Models;
using BlazorEngine.Utils;

namespace TestShared.Views
{
  [Route("card2")]
  [AddToMenu(Title = "Card Page 2", Route = "card2", Group = "Sub Menu")]
  public class SubMenu2 : CardPage<Mock>
  {
    protected override Task LoadVisibleFields()
    {
      VisibleFields = [];
      VisibleFields.AddField(nameof(Mock.Id));

      VisibleFields.AddField(nameof(Mock.Name)).AddFieldProperty(t => t.OnLookup = (_) => new Dictionary<object, string> { { "test", "test view" }, { "test1", "test view 1" } });
      VisibleFields.AddField(nameof(Mock.Price)).AddFieldProperty(prop => prop.OnDrillDown = args => UIServices.DialogService.ShowInfo("DrillDown"));
      VisibleFields.AddField(nameof(Mock.Description), (ref VisibleField<Mock> o) => o.TextFieldType = Microsoft.FluentUI.AspNetCore.Components.TextFieldType.Password);
      VisibleFields.AddField(nameof(Mock.OrderDate)).AddFieldProperty(prop => prop.OnDrillDown = args => UIServices.DialogService.ShowInfo("DrillDown"));
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
    public async Task ShowProgress()
    {
      UIServices!.ProgressService.StartProgress();
      await Task.Delay(100000);
      UIServices!.ProgressService.StopProgress();
    }

    [PageAction(Caption = "Open Modal")]
    public async Task OpenModal()
    {
      var mock = Mock.GetSingleMock();
      _ = await UIServices!.OpenModal(typeof(ModalView), mock);
    }

    [PageAction(Caption = "Ask User")]
    public async Task AskUser()
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
    public async Task UploadFiles()
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

    [PageAction(Caption = "No Cancellation", Group = "grouped")]
    public async Task Test1()
    {
      await LongProcedure();
    }
    [PageAction(Caption = "With Cancellation", Group = "grouped")]
    public async Task Test2()
    {
      await LongProcedure();
    }

    async Task LongProcedure()
    {
      await Task.Delay(5000);
      UIServices.DialogService.ShowInfo("Done");
    }

    [PageAction(Caption = "Thorow Error")]
    public void Test3()
    {
      throw new NotImplementedException();
    }
    [PageAction(Caption = "Lock UI")]
    public async Task Test4()
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
