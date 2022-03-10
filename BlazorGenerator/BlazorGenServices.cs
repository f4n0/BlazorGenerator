using BlazorGenerator.Services;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator
{
  public static class BlazorGenServices
  {
    public static IServiceCollection AddBlazorGen(this IServiceCollection services)
    {
      services
      .AddBlazorise(options =>
      {
        options.Immediate = false;
      })
      .AddBootstrapProviders();
      services.AddFontAwesomeIcons();

      services.AddSingleton<BlazorGenLogger>();
      return services;
    }
  }
}
