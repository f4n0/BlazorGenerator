using BlazorGenerator.Attributes;
using BlazorGenerator.Layouts;
using BlazorGenerator.Models;
using BlazorGenerator.Utils;
using Microsoft.AspNetCore.Components;
using Server.Data;

namespace Server.Views
{
    [Route("/list")]
    [AddToMenu(Title = "List Page", Route= "/list")]
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

            Data = Mock.getMultipleMock().AsQueryable();
        }

        public override void Save(Mock Rec, Mock xRec)
        {
            var tmp = Data.ToList();
            tmp[Data.ToList().FindIndex(o => o.Id == xRec.Id)] = Rec;
            Data = tmp.AsQueryable();
        }
    }
}
