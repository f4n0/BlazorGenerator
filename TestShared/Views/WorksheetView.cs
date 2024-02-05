using BlazorGenerator.Attributes;
using BlazorGenerator.Layouts;
using BlazorGenerator.Models;
using BlazorGenerator.Utils;
using Microsoft.AspNetCore.Components;
using TestShared.Data;

namespace TestShared.Views
{
  [Route("WorksheetView")]
  [Route("WorksheetView/{param}")]
  [AddToMenu(Title = "Worksheet Page", Route = "WorksheetView")]
  public class WorksheetView : Worksheet<Mock, WorksheetView>
  {
    [Parameter]
    public string param { get; set; }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public override string Title => "List View";


    protected override async Task LoadData()
    {
      Content = Mock.getSingleMock();
      ListContent = null;
    }

    protected override async Task LoadVisibleFields()
    {
      VisibleFields = new List<VisibleField<Mock>>();
      VisibleFields.AddField(nameof(Mock.Id));
      VisibleFields.AddField(nameof(Mock.Name));
      VisibleFields.AddField(nameof(Mock.Price));
      VisibleFields.AddField(nameof(Mock.Description));
      VisibleFields.AddField(nameof(Mock.OrderDate));


      ListVisibleFields = new List<VisibleField<WorksheetView>>();
      ListVisibleFields.AddField(nameof(Id));
      ListVisibleFields.AddField(nameof(Name));
      ListVisibleFields.AddField(nameof(Description));
    }

  }
}
