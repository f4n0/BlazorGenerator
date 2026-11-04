using System.Diagnostics.CodeAnalysis;
using BlazorEngine.Actions;
using BlazorEngine.Fields;
using BlazorEngine.Layouts;
using BlazorEngine.Models;
using BlazorEngine.Services;
using BlazorEngine.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using TestShared.Data;

namespace TestShared.Views;

[Route(Route)]
public class CardView : CardPage<Mock>
{
  public const string Route = "card";
  public override string Title => "br-labsdev2 Integration - Apps";

  [Inject] public required BackgroundExecutor BackgroundExecutor { get; set; }

  protected override Task ConfigureFields(FieldListBuilder<Mock> fields)
  {
    fields.Add(x => x.Id);
    fields.Add(x => x.Name)
      .Tooltip("Lorem ipsum dolor sit admet, qui really long text!")
      .Required()
      .OnLookup(_ => new Dictionary<object, string>
        { { "test", "test <i>view</i>" }, { "test1", "test view 1" } });
    fields.Add(x => x.Price).OnDrillDown(args => UIServices.DialogService.ShowInfoAsync("DrillDown"));
    fields.Add(x => x.Description).TextInputType(TextInputType.Password).Additional();

    fields.Add(x => x.Name).Multiline();
    fields.Add(x => x.OrderDate).OnDrillDown(args => UIServices.DialogService.ShowInfoAsync("DrillDown"));
    fields.Add(x => x.Type);
    fields.Add(x => x.NullTest).Additional();
    fields.Add(x => x.Enabled).Additional();
    fields.Custom("test custom", (data, field) => builder =>
    {
      var value = true;
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

    fields.Add(new VisibleField<Mock>().Configure(field => field
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

  protected override IReadOnlyList<PageActionDefinition> CreatePageActions() =>
  [
    new("ShowProgress", ShowProgress),
    new("Open Modal", OpenModal),
    new("Ask User", AskUser),
    new("Show Upload", UploadFiles),
    new("Enqueue Action", Enqueue),
    new("Go To", GoTo),
    new("No Cancellation", Test1, group: "grouped"),
    new("With Cancellation", Test2, group: "grouped"),
    new("Thorow Error", Test3),
    new("Lock UI", Test4),
    new("Write Log", Test5Async),
    new("Test6", Test6)
  ];

  public async Task ShowProgress()
  {
    UIServices!.ProgressService.StartProgress();
    await Task.Delay(1000);
    UIServices!.ProgressService.StopProgress();
    StateHasChanged();
  }

  public async Task OpenModal()
  {
    var mock = Mock.GetSingleMock();
    var res = await UIServices!.OpenModal(typeof(ModalView), mock);
  }

  public async Task AskUser()
  {
    _ = await UIServices!.UserInput(new UserInputData
    {
      Message = "Gimme the data"
    });

    _ = await UIServices!.UserInput(new UserInputData
    {
      Message = "Gimme the Password",
      InputType = UserInputType.Secret
    });

    _ = await UIServices!.UserInput(new UserInputData
    {
      Message = "Choose!",
      InputType = UserInputType.Choice,
      Choices = ["Uno", "Due", "Tre"]
    });
  }

  public async Task UploadFiles()
  {
    try
    {
      var test = await UIServices.UploadFile();
      if (test?.Files != null)
        foreach (var file in test.Files)
          _ = file;
    }
    catch (Exception e)
    {
     await UIServices.ShowErrorAsync(e.Message);
    }
  }

  public async Task Enqueue()
  {
    BackgroundExecutor.QueueAction("Test", async (logger, ct) =>
    {
      
      logger.LogInformation("Starting long action");
      await Task.Delay(1000, ct);
      logger.LogInformation("1: long action");
      await Task.Delay(1000, ct);
      logger.LogInformation("2: long action");
      await Task.Delay(1000, ct);
      logger.LogInformation("3: long action");
      await Task.Delay(1000, ct);
      logger.LogInformation("4: long action");
      await Task.Delay(1000, ct);
      logger.LogInformation("5: long action");
      await Task.Delay(1000, ct);
      logger.LogInformation("6: long action");
      await Task.Delay(1000, ct);
      logger.LogInformation("7: long action");
      logger.LogTrace("This is a trace log");
      logger.LogCritical("This is a critical log");
      logger.LogDebug("This is a debug log");
      logger.LogError("This is an error log");
      logger.LogWarning("This is a warning log");

      logger.LogInformation(
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
      logger.LogInformation("Finished long action");
    });
  }

  public void GoTo()
  {
    NavManager.NavigateTo("WorksheetView", true);
  }

  public async Task Test1()
  {
    await LongProcedure();
  }

  public async Task Test2()
  {
    await LongProcedure(ComponentDetached);
  }

  private async Task LongProcedure(CancellationToken cy = default)
  {
    await Task.Delay(5000, cy);
    await UIServices.DialogService.ShowInfoAsync("Done");
  }

  public void Test3()
  {
    throw new NotImplementedException();
  }

  public async Task Test4()
  {
    UIServices.LockUI();
    await Task.Delay(2000);
    UIServices.UnlockUI();
  }

  public async Task Test5Async()
  {
    await Task.Delay(1000);
    for (var i = 0; i < 30; i++)
    {
      await Task.Delay(50);
      UIServices.Logger.SendLogMessage($"Test{i}");
    }
  }

  [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Test Only")]
  public void Test6()
  {
  }
}
