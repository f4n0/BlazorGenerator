using Microsoft.AspNetCore.Components;
using TestShared.Data;
using BlazorEngine.Attributes;
using BlazorEngine.Layouts;
using BlazorEngine.Models;
using BlazorEngine.Utils;

namespace TestShared.Views
{
  [Route("card")]
  [AddToMenu(Title = "Card Page", Route = "card")]
  public class CardView : CardPage<Mock>
  {
    public override string Title => "br-labsdev2 Integration - Apps";
    protected override Task LoadVisibleFields()
    {
      VisibleFields = [];
      VisibleFields.AddField(nameof(Mock.Id));
      VisibleFields.AddField(nameof(Mock.test))
        .AddFieldProperty(p => p.Required = true)
        .AddFieldProperty(p => p.EnableSearch = true)
        .AddFieldProperty(t => t.OnLookup = (_) => new Dictionary<object, string> { { "test", "test <i>view</i>" }, { "test1", "test view 1" } });

      VisibleFields.AddField(nameof(Mock.Name))
        .AddFieldProperty(p=> p.Tooltip = "Lorem ipsum dolor sit admet, qui really long text!")
        .AddFieldProperty(p=> p.Required = true)
        .AddFieldProperty(t => t.OnLookup = (_) => new Dictionary<object, string> { { "test", "test <i>view</i>" }, { "test1", "test view 1" } });
      VisibleFields.AddField(nameof(Mock.Price)).AddFieldProperty(prop => prop.OnDrillDown = args => UIServices.DialogService.ShowInfo("DrillDown"));
      VisibleFields.AddField(nameof(Mock.Description), (ref VisibleField<Mock> o) => {
        o.TextFieldType = Microsoft.FluentUI.AspNetCore.Components.TextFieldType.Password;
        o.Additional = true;
        }
      );

      VisibleFields.AddField(nameof(Mock.Name)).AddFieldProperty(p => p.Multiline = true);
      VisibleFields.AddField(nameof(Mock.OrderDate)).AddFieldProperty(prop => prop.OnDrillDown = args => UIServices.DialogService.ShowInfo("DrillDown"));
      VisibleFields.AddField(nameof(Mock.Type));
      VisibleFields.AddField(nameof(Mock.NullTest)).AddFieldProperty(p => p.Additional = true);
      VisibleFields.AddField(nameof(Mock.Enabled)).AddFieldProperty(p => p.Additional = true);
      VisibleFields.AddCustomField("test custom", (data, field) => builder =>
      {
        bool value = true;
        var seq = 0;
        builder.OpenElement(seq, "fluent-checkbox");
        builder.AddAttribute(++seq, "Value", value);
        builder.AddAttribute(++seq, "ValueChanged", onValueChanged);

        void onValueChanged(object e)
        {
          value = (bool)e;
        }
        builder.CloseElement();
        
      });
      
      VisibleFields.Add(new VisibleField<Mock>().Configure(field => field
        .FieldType(typeof(string))
        .Name("stani")
        .Caption("stani cap")
        .Get(args => "test field")
        .ReadOnly()
      ));
      
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

    [PageAction(Caption = "No Cancellation", Group = "grouped")]
    public async Task Test1()
    {
      await LongProcedure();
    }
    [PageAction(Caption = "With Cancellation", Group = "grouped")]
    public async Task Test2()
    {
      await LongProcedure(ComponentDetached);
    }

    async Task LongProcedure(CancellationToken cy = default)
    {
      await Task.Delay(5000, cy);
      UIServices.DialogService.ShowInfo("Done");
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
