using BlazorEngine.Services;
using Microsoft.JSInterop;

namespace TestShared.Services;

public class HelpService : IHelpService
{
  private readonly IJSRuntime _jsRuntime;

  public HelpService(IJSRuntime jsRuntime)
  {
    _jsRuntime = jsRuntime;
  }

  public void GetHelp(object sender)
  {
    _jsRuntime.InvokeVoidAsync("open", "https://github.com/f4n0/BlazorGenerator", "_blank");
  }
}