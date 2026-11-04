using System.Diagnostics.CodeAnalysis;
using BlazorEngine.Actions;
using BlazorEngine.Fields;
using BlazorEngine.Layouts;
using BlazorEngine.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using TestShared.Data;

namespace TestShared.Views;

[Route(Route)]
public class SubMenu2 : CardPage<Mock>
{
  public const string Route = "card2";

  protected override Task ConfigureFields(FieldListBuilder<Mock> fields)
  {
    fields.Add(x => x.Id);

    fields.Add(x => x.Name)
      .OnLookup(_ => new Dictionary<object, string> { { "test", "test view" }, { "test1", "test view 1" } });
    fields.Add(x => x.Price).OnDrillDown(args => UIServices.DialogService.ShowInfoAsync("DrillDown"));
    fields.Add(x => x.Description).TextInputType(TextInputType.Password);
    fields.Add(x => x.OrderDate).OnDrillDown(args => UIServices.DialogService.ShowInfoAsync("DrillDown"));
    fields.Add(x => x.Type);
    fields.Add(x => x.NullTest);

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
    new("Go To", GoTo),
    new("No Cancellation", Test1, group: "grouped"),
    new("With Cancellation", Test2, group: "grouped"),
    new("Thorow Error", Test3),
    new("Lock UI", Test4),
    new("Write Log", Test5),
    new("Test6", Test6)
  ];

  public async Task ShowProgress()
  {
    UIServices!.ProgressService.StartProgress();
    await Task.Delay(100000);
    UIServices!.ProgressService.StopProgress();
  }

  public async Task OpenModal()
  {
    var mock = Mock.GetSingleMock();
    _ = await UIServices!.OpenModal(typeof(ModalView), mock);
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
    var test = await UIServices.UploadFile();
    if (test?.Files != null)
      foreach (var file in test.Files)
        _ = file;
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
    await LongProcedure();
  }

  private async Task LongProcedure()
  {
    await Task.Delay(5000);
    UIServices.DialogService.ShowInfoAsync("Done");
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

  public void Test5()
  {
    for (var i = 0; i < 30; i++) UIServices.Logger.SendLogMessage($"Test{i}");
  }

  [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Test Only")]
  public void Test6()
  {
  }
}
