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
    public static IServiceCollection AddBlazorGen(this IServiceCollection services, Action<BlazorGenOptions> configureOptions = null)
    {
      services
      .AddBlazorise(options =>
      {
        options.Immediate = false;
        options.ThemeCacheSize = 0;
      })
      .AddBootstrapProviders();
      services.AddFontAwesomeIcons();

      services.AddSingleton<BlazorGenLogger>();


      configureOptions ??= _ => { };
      services.AddSingleton(configureOptions);
      services.AddSingleton<BlazorGenOptions>();


      return services;
    }


  }
}
