using BlazorGenerator.Attributes;
using BlazorGenerator.Layouts;
using BlazorGenerator.Models;
using BlazorGenerator.Utils;
using Microsoft.AspNetCore.Components;
using Server.Data;

namespace Server.Views
{
    [Route("/WorksheetView")]
    [AddToMenu(Title = "Worksheet Page", Route= "/WorksheetView")]
    public class WorksheetView : Worksheet<Mock, Mock>
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

            Data = Mock.getSingleMock();


            ListVisibleFields = new List<VisibleField<Mock>>();
            ListVisibleFields.AddField(nameof(Mock.Id));
            ListVisibleFields.AddField(nameof(Mock.Name));
            ListVisibleFields.AddField(nameof(Mock.Price));
            ListVisibleFields.AddField(nameof(Mock.Description));
            ListVisibleFields.AddField(nameof(Mock.OrderDate));

            ListData = Mock.getMultipleMock().AsQueryable();
        }

        public override void ListSave(Mock Rec, Mock xRec)
        {
            var tmp = ListData.ToList();
            tmp[ListData.ToList().FindIndex(o => o.Id == xRec.Id)] = Rec;
            ListData = tmp.AsQueryable();
        }
    }
}
