using BlazorEngine.Layouts;
using BlazorEngine.Fields;
using TestShared.Data;

namespace TestShared.Views;

public class ModalView : CardPage<Mock>
{
  public override string Title => "List View";

  protected override Task ConfigureFields(FieldListBuilder<Mock> fields)
  {
    fields.Add(x => x.Id);
    fields.Add(x => x.Name);
    fields.Add(x => x.Price);
    fields.Add(x => x.Description);
    fields.Add(x => x.OrderDate);
    fields.Add(x => x.Type);

    ShowActions = false;
    ShowButtons = false;
    return Task.CompletedTask;
  }
}
