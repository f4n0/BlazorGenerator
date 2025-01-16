using BlazorEngine.Security;
using BlazorEngine.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorEngine.Components.Base
{
  public partial class BlazorEngineComponentBase : ComponentBase
  {
    [Inject]
    public NavigationManager NavManager { get; set; } = null!;

    [Inject]
    public UIServices UIServices { get; set; } = null!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = null!;

    [Inject]
    internal BlazorEngineSecurityService Security { get; set; } = null!;

  }
}
