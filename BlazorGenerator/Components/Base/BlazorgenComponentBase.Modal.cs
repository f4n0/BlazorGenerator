using BlazorGenerator.Components.Modal;
using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Components.Base
{
    public partial class BlazorgenComponentBase : ComponentBase, IDisposable
    {
        [Parameter]
        public bool isModal { get; set; }

        public void OpenModal<TModalData>(Type Page, TModalData modalData)
        {            
            if(modal is  null) {
                modal = new BlazorGeneratorModal();
            }
            modal.ChildContent = new RenderFragment(builder =>
            {
                builder.OpenComponent(0, Page);
                builder.AddAttribute(1, "isModal", true);
                builder.CloseComponent();
            });

            modal.Show();
        }
    }
}
