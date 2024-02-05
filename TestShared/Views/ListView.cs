using BlazorGenerator.Attributes;
using BlazorGenerator.Layouts;
using BlazorGenerator.Models;
using BlazorGenerator.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using TestShared.Data;

namespace TestShared.Views
{
  [Route("list")]
  [AddToMenu(Title = "List Page", Route = "list", Icon = typeof(Icons.Regular.Size16.AddSquare))]
  public class ListView : ListPage<Mock>
  {
    public override string Title => "List View";

    protected override async Task LoadVisibleFields()
    {
      VisibleFields = [];
      VisibleFields.AddField(nameof(Mock.Id), (ref VisibleField<Mock> prop) =>
      {
        prop.Href = (data) => "/test/" + data.Id;
      });
      VisibleFields.AddField(nameof(Mock.Name));
      VisibleFields.AddField(nameof(Mock.Price));
      VisibleFields.AddField(nameof(Mock.Description));
      VisibleFields.AddField(nameof(Mock.OrderDate));
      VisibleFields.AddField(nameof(Mock.icon));
    }

    protected override async Task LoadData()
    {
      Content = Mock.getMultipleMock().AsQueryable();
    }


    public void Save(Mock Rec, Mock xRec)
    {
      var tmp = Content!.ToList();
      tmp[Content!.ToList().FindIndex(o => o.Id == xRec.Id)] = Rec;
      Content = tmp.AsQueryable();
    }

    [PageAction]
    public void PageAction()
    {
      UIServices!.DialogService.ShowInfo("Page Action");
    }

    [GridAction(Caption = "Install", GridIcon = typeof(Icons.Regular.Size16.AirplaneTakeOff))]
    public void GridAction()
    {
      UIServices!.DialogService.ShowInfo("Grid Action");
    }
  }
}
