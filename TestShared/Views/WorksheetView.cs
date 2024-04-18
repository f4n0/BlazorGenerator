using BlazorGenerator.Attributes;
using BlazorGenerator.Layouts;
using BlazorGenerator.Models;
using BlazorGenerator.Utils;
using Microsoft.AspNetCore.Components;
using TestShared.Data;

namespace TestShared.Views
{
  [Route("WorksheetView")]
  [Route("WorksheetView/{Param}")]
  [AddToMenu(Title = "Worksheet Page", Route = "WorksheetView")]
  public class WorksheetView : Worksheet<Mock, Mock>
  {
    [Parameter]
    public string? Param { get; set; }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public override string Title => "List View";

    protected override Task LoadData()
    {
      Content = Mock.GetSingleMock();
      ListContent = Mock.GetMultipleMock(3);
      return Task.CompletedTask;
    }

    protected override Task LoadVisibleFields()
    {
      VisibleFields = [];
      VisibleFields.AddField(nameof(Mock.Id));
      VisibleFields.AddField(nameof(Mock.Name));
      VisibleFields.AddField(nameof(Mock.Price));
      VisibleFields.AddField(nameof(Mock.Description));
      VisibleFields.AddField(nameof(Mock.OrderDate));

      ListVisibleFields = [];
      ListVisibleFields.AddField(nameof(Mock.Id));
      ListVisibleFields.AddField(nameof(Mock.Name)).AddFieldProperty(prop => prop.TextStyle = BlazorGenerator.Enum.TextStyle.Italic);
      ListVisibleFields.AddField(nameof(Mock.Description));
      return Task.CompletedTask;
    }
  }
}
