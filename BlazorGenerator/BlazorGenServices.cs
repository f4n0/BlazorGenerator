using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.Extensions.DependencyInjection;
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
        options.ChangeTextOnKeyPress = false;
      })
      .AddBootstrapProviders();
      services.AddFontAwesomeIcons();
      return services;
    }
  }
}
