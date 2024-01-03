using BlazorGenerator.Components.Modal;
using BlazorGenerator.Security;
using BlazorGenerator.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorGenerator.Components.Base
{
    public partial class BlazorgenComponentBase : ComponentBase, IDisposable
    {


        public virtual string Title => "";

        public virtual bool ShowButtons { get; set; } = true;
        public virtual bool ShowActions { get; set; } = true;


        public BlazorGeneratorModal modal { get; set; }

        public void Dispose()
        {
        }
    }
}
