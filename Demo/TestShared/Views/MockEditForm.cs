using BlazorGenerator.Attributes;
using BlazorGenerator.Layouts;
using BlazorGenerator.Models;
using BlazorGenerator.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using TestShared.Data;

namespace TestShared.Views
{
  public class MockEditForm : CardPage<Mock>
  {
    public override string Title => "List View";
    public override int GridSize => 12;
    public override bool ShowButtons => false;
    public override bool ShowActions => false;

    protected override Task LoadVisibleFields()
    {
      VisibleFields = [];
      VisibleFields.AddField(nameof(Mock.Id));
      VisibleFields.AddField(nameof(Mock.Name)).AddFieldProperty(pro =>
      {
        pro.Get = (args) =>
        {
          return args.Data.Name;
        };
      });
      VisibleFields.AddField(nameof(Mock.Price));
      VisibleFields.AddField(nameof(Mock.Description));
      VisibleFields.AddField(nameof(Mock.OrderDate));
      VisibleFields.AddField(nameof(Mock.Icon));
      VisibleFields.AddField(nameof(Mock.Type));
      VisibleFields.AddField(nameof(Mock.Enabled));
      return Task.CompletedTask;
    }

  }
}
