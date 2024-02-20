using BlazorGenerator.Security;
using BlazorGenerator.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorGenerator.Components.Base
{
  public partial class BlazorgenComponentBase : ComponentBase, IDisposable
  {
    [Inject]
    public NavigationManager? NavManager { get; set; }

    [Inject]
    public UIServices? UIServices { get; set; }

    [Inject]
    public IJSRuntime? JSRuntime { get; set; }

    [Inject]
    internal BlazorGeneratorSecurity? Security { get; set; }
  }
}
