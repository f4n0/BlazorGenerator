using BlazorEngine.Layouts;
using BlazorEngine.Fields;
using TestShared.Data;

namespace TestShared.Views;

public class MockEditForm : CardPage<Mock>
{
  public override string Title => "List View";
  public override int GridSize => 12;
  public override bool ShowButtons => false;
  public override bool ShowActions => false;

  protected override Task ConfigureFields(FieldListBuilder<Mock> fields)
  {
    fields.Add(x => x.Id);
    fields.Add(x => x.Name).Get(args => args.Data.Name);
    fields.Add(x => x.Price);
    fields.Add(x => x.Description);
    fields.Add(x => x.OrderDate);
    fields.Add(x => x.Icon);
    fields.Add(x => x.Type);
    fields.Add(x => x.Enabled);
    return Task.CompletedTask;
  }
}
