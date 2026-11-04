using BlazorEngine.Fields;
using BlazorEngine.Layouts;
using Microsoft.AspNetCore.Components;
using TestShared.Data;

namespace TestShared.Views;

[Route(Route)]
public class TwoListView : TwoList<Mock, Mock>
{
  public const string Route = "twolist";
  public override string Title => "List View";
  public override Type? SecondListEditFormType => typeof(MockEditForm);
  public override Type? FirstListEditFormType => typeof(MockEditForm);

  protected override Task ConfigureFirstListFields(FieldListBuilder<Mock> fields)
  {
    fields.Add(x => x.Id);
    fields.Add(x => x.Name);
    fields.Add(x => x.Price);
    fields.Add(x => x.Description);
    fields.Add(x => x.OrderDate);
    return Task.CompletedTask;
  }

  protected override Task ConfigureSecondListFields(FieldListBuilder<Mock> fields)
  {
    fields.Add(x => x.Id);
    fields.Add(x => x.Name);
    return Task.CompletedTask;
  }

  protected override Task LoadData()
  {
    FirstListContent = Mock.GetMultipleMock();
    SecondListContent = Mock.GetMultipleMock();
    return Task.CompletedTask;
  }
}
