using Blazorise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator
{
  public class BlazorGenOptions
  {
    private readonly IServiceProvider serviceProvider;

    public BlazorGenOptions(IServiceProvider serviceProvider, Action<BlazorGenOptions> configureOptions)
    {
      this.serviceProvider = serviceProvider;
      configureOptions?.Invoke(this);
    }

    public bool ShowBreaddcrumbs { get; set; } = true;


    public IServiceProvider Services => serviceProvider;
  }
}
