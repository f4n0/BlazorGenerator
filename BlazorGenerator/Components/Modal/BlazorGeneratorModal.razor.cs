using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Components.Modal
{
    public partial class BlazorGeneratorModal : ComponentBase
    {
        private FluentDialog? FluentDialog;        
        private bool Hidden { get; set; } = true;

        [Parameter]
        public RenderFragment ChildContent { get; set; }



        public void Show()
        {
            FluentDialog!.Show();
        }

        public void Hide()
        {
            FluentDialog!.Hide();
        }


        private void OnDismiss(DialogEventArgs args)
        {
            if (args is not null && args.Reason is not null && args.Reason == "dismiss")
            {
                FluentDialog!.Hide();
            }
        }

    }
}
