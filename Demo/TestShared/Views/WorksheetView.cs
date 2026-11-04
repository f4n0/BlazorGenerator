using System.Collections.Specialized;
using BlazorEngine.Actions;
using BlazorEngine.Fields;
using BlazorEngine.Enum;
using BlazorEngine.Layouts;
using Microsoft.AspNetCore.Components;
using TestShared.Data;

namespace TestShared.Views;

[Route(Route)]
[Route(Route + "/{Param}")]
public class WorksheetView : Worksheet<Mock, Mock>
{
  public const string Route = "WorksheetView";
  [Parameter] public string? Param { get; set; }

  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;

  public override bool ShowExportToExcel => false;
  public override string Title => "List View";
  public override Type? ListEditFormType => typeof(MockEditForm);

  protected override Task LoadData()
  {
    ListSelected.CollectionChanged += SelectedChanged;
    Content = Mock.GetSingleMock();
    ListContent = Mock.GetMultipleMock(30);
    return Task.CompletedTask;
  }

  private void SelectedChanged(object? sender, NotifyCollectionChangedEventArgs e)
  {
    var selected = e.NewItems?.OfType<Mock>().FirstOrDefault();
    if (selected != null)
    {
      Content = selected;
      RefreshCard();
    }
  }

  protected override Task ConfigureFields(FieldListBuilder<Mock> fields)
  {
    ShowButtons = false;

    fields.Add(x => x.Id);
    fields.Add(x => x.Name);
    fields.Add(x => x.Price);
    fields.Add(x => x.Description);
    fields.Add(x => x.OrderDate);
    return Task.CompletedTask;
  }

  protected override Task ConfigureListFields(FieldListBuilder<Mock> fields)
  {
    fields.Add(x => x.Id);
    fields.Add(x => x.Name).TextStyle(_ => TextStyle.Italic);
    fields.Add(x => x.Description);
    return Task.CompletedTask;
  }

  protected override IReadOnlyList<PageActionDefinition> CreatePageActions() => [new("Test", Test)];

  public void Test()
  {
    ListContent = Mock.GetMultipleMock(2);
    StateHasChanged();
  }
}
