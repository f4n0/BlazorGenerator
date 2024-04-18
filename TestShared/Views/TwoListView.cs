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

    protected override Task LoadVisibleFields()
    {
      FirstListVisibleFields = [];
      FirstListVisibleFields.AddField(nameof(Mock.Id));
      FirstListVisibleFields.AddField(nameof(Mock.Name));
      FirstListVisibleFields.AddField(nameof(Mock.Price));
      FirstListVisibleFields.AddField(nameof(Mock.Description));
      FirstListVisibleFields.AddField(nameof(Mock.OrderDate));

      SecondListVisibleFields = [];
      SecondListVisibleFields.AddField(nameof(Mock.Id));
      SecondListVisibleFields.AddField(nameof(Mock.Name));
      return Task.CompletedTask;
    }

    protected override Task LoadData()
    {
      FirstListContent = Mock.GetMultipleMock();
      SecondListContent = Mock.GetMultipleMock();
      return Task.CompletedTask;
    }
  }
}
