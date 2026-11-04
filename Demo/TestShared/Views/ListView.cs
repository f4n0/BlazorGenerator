using System.Diagnostics.CodeAnalysis;
using BlazorEngine.Actions;
using BlazorEngine.Fields;
using BlazorEngine.Layouts;
using Microsoft.AspNetCore.Components;
using TestShared.Data;
using static Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size16;

namespace TestShared.Views;

[Route(Route)]
public class ListView : ListPage<Mock>
{
  public const string Route = "list";
  public override string Title => "List View";
  public override Type? EditFormType => typeof(MockEditForm);
  public override bool UseVirtualization => true;

  protected override Task ConfigureFields(FieldListBuilder<Mock> fields)
  {
    fields.Add(x => x.Id).Href(data => "/test/" + data.Id);
    fields.Add(x => x.Name).Get(args => args.Data.Name);
    fields.Add(x => x.Price).GetAsync(args => DelayedNameAsync(args.Data));

    async Task<object?> DelayedNameAsync(Mock data)
    {
      await Task.Delay(TimeSpan.FromSeconds(2));
      return data.Name;
    }

    fields.Add(x => x.Description);
    fields.Add(x => x.OrderDate);
    fields.Add(x => x.Icon);
    fields.Add(x => x.Type);
    fields.Add(x => x.Enabled);
    return Task.CompletedTask;
  }


  protected override Task LoadData()
  {
    Content = Mock.GetMultipleMock().OrderBy(p => p.Id);
    return Task.CompletedTask;
  }

  public override void OnSave(Mock? entity)
  {
    if (entity == null)
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

  protected override IReadOnlyList<PageActionDefinition> CreatePageActions() =>
  [
    new("Refresh", Refresh),
    new("Add Rows", PageAction),
    new("Remove Rows", RemoveRows),
    new("Install", () => GridAction()),
    new("ShowProgress", ShowProgress),
    new("Open Modal", OpenModal),
    new("Go To", GoTo),
    new("Test1", Test1, group: "grouped"),
    new("Test2", Test2, group: "grouped")
  ];

  protected override IReadOnlyList<GridActionDefinition<Mock>> CreateGridActions() =>
  [
    new("Install", GridAction, new AirplaneTakeOff()),
    new("ShowProgress", _ => ShowProgress())
  ];

  public async Task Refresh()
  {
    await LoadData();
    StateHasChanged();
  }

  public async Task PageAction()
  {
    var test = Content?.ToList() ?? new List<Mock>();
    test.AddRange(Mock.GetMultipleMock());
    Content = test;
    StateHasChanged();
  }

  public async Task RemoveRows()
  {
    Content = null;
    StateHasChanged();
  }

  public void GridAction(Mock? rec = null)
  {
    _ = rec;
    UIServices!.DialogService.ShowInfoAsync("Grid Action");
  }

  public async Task ShowProgress()
  {
    UIServices!.ProgressService.StartProgress();
    await Task.Delay(10000);
    UIServices!.ProgressService.StopProgress();
  }

  public async Task OpenModal()
  {
    var mock = Mock.GetSingleMock();
    _ = await UIServices!.OpenModal(typeof(ModalView), mock);
  }

  public void GoTo()
  {
    NavManager.NavigateTo("WorksheetView", true);
  }

  public async Task Test1()
  {
    var mock = Mock.GetSingleMock();
    _ = await UIServices!.OpenModal(typeof(ModalView), mock);
  }

  [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Test Only")]
  public void Test2()
  {
  }
}
