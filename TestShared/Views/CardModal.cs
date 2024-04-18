using BlazorGenerator.Layouts;
using BlazorGenerator.Models;
using BlazorGenerator.Utils;
using TestShared.Data;

namespace TestShared.Views
{
  public class ModalView : CardPage<Mock>
  {
    public override string Title => "List View";

    protected override Task LoadVisibleFields()
    {
      VisibleFields = [];
      VisibleFields.AddField(nameof(Mock.Id));
      VisibleFields.AddField(nameof(Mock.Name));
      VisibleFields.AddField(nameof(Mock.Price));
      VisibleFields.AddField(nameof(Mock.Description));
      VisibleFields.AddField(nameof(Mock.OrderDate));
      VisibleFields.AddField(nameof(Mock.Type));

      ShowActions = false;
      ShowButtons = false;
      return Task.CompletedTask;
    }
  }
}
