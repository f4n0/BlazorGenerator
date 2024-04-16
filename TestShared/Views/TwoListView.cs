using BlazorGenerator.Attributes;
using BlazorGenerator.Layouts;
using BlazorGenerator.Models;
using BlazorGenerator.Utils;
using Microsoft.AspNetCore.Components;
using TestShared.Data;

namespace TestShared.Views
{
  [Route("twolist")]
  [AddToMenu(Title = "TwoList Page", Route = "twolist")]
  public class TwoListView : TwoList<Mock, Mock>
  {
    public override string Title => "List View";

    protected override async Task LoadVisibleFields()
    {
      FirstListVisibleFields = new List<VisibleField<Mock>>();
      FirstListVisibleFields.AddField(nameof(Mock.Id));
      FirstListVisibleFields.AddField(nameof(Mock.Name));
      FirstListVisibleFields.AddField(nameof(Mock.Price));
      FirstListVisibleFields.AddField(nameof(Mock.Description));
      FirstListVisibleFields.AddField(nameof(Mock.OrderDate));


      SecondListVisibleFields = new List<VisibleField<Mock>>();
      SecondListVisibleFields.AddField(nameof(Mock.Id));
      SecondListVisibleFields.AddField(nameof(Mock.Name));
    }

    protected override async Task LoadData()
    {
      FirstListContent = Mock.getMultipleMock();
      SecondListContent = Mock.getMultipleMock();
    }
  }
}
