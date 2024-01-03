using BlazorGenerator.Attributes;
using BlazorGenerator.Layouts;
using BlazorGenerator.Models;
using BlazorGenerator.Utils;
using Microsoft.AspNetCore.Components;
using Server.Data;

namespace Server.Views
{
    [Route("/card")]
    [AddToMenu(Title = "Card Page", Route= "/card")]
    public class CardView : CardPage<Mock>
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
            VisibleFields.AddField(nameof(Mock.type));

            Data = Mock.getSingleMock();
        }


        [PageAction(Caption = "ShowProgress")]
        public async void ShowProgress()
        {
            UIServices.progressService.StartProgress();
            await Task.Delay(10000);
            UIServices.progressService.StopProgress();
        }

        [PageAction(Caption = "Open Modal")]
        public async void OpenModal()
        {
            OpenModal<Mock>(typeof(CardView), new Mock());
        }


    }
}
