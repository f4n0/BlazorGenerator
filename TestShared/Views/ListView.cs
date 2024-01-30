using BlazorGenerator.Attributes;
using BlazorGenerator.Layouts;
using BlazorGenerator.Models;
using BlazorGenerator.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using TestShared.Data;

namespace TestShared.Views
{
  [Route("/list")]
  [AddToMenu(Title = "List Page", Route = "/list", Icon = typeof(Icons.Regular.Size16.AddSquare))]
  public class ListView : ListPage<Mock>
  {

    public override string Title => "List View";



    protected override void OnParametersSet()
    {
      VisibleFields = new List<VisibleField<Mock>>();
      VisibleFields.AddField(nameof(Mock.Id));
      VisibleFields.AddField(nameof(Mock.Name));
      VisibleFields.AddField(nameof(Mock.Price));
      VisibleFields.AddField(nameof(Mock.Description));
      VisibleFields.AddField(nameof(Mock.OrderDate));

      Content = Mock.getMultipleMock().AsQueryable();
    }

    public void Save(Mock Rec, Mock xRec)
    {
      var tmp = Content.ToList();
      tmp[Content.ToList().FindIndex(o => o.Id == xRec.Id)] = Rec;
      Content = tmp.AsQueryable();
    }

    [PageAction]
    public void PageAction()
    {
      UIServices.dialogService.ShowInfo("Page Action");
    }

    [GridAction(Caption = "Install", GridIcon = typeof(Icons.Regular.Size16.AirplaneTakeOff))]
    public void GridAction(Mock Rec)
    {
      UIServices.dialogService.ShowInfo("Grid Action");
    }

  }
}
