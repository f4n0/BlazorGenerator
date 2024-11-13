using BlazorGenerator.Security;
using BlazorGenerator.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorGenerator.Components.Base
{
  public partial class BlazorGeneratorComponentBase : ComponentBase, IDisposable
  {
    [Inject]
    public NavigationManager NavManager { get; set; } = null!;

    [Inject]
    public UIServices UIServices { get; set; } = null!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = null!;

    [Inject]
    internal BlazorGeneratorSecurityService Security { get; set; } = null!;

  }
}
