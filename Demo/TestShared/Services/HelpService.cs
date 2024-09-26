using BlazorGenerator.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestShared.Services
{
  public class HelpService : IHelpService
  {
    IJSRuntime JSRuntime;
    public HelpService(IJSRuntime IJSRuntime)
    {
      JSRuntime = IJSRuntime;
    }
    public void GetHelp(object sender)
    {
      JSRuntime.InvokeVoidAsync("open", "https://github.com/f4n0/BlazorGenerator", "_blank");
    }
  }
}
