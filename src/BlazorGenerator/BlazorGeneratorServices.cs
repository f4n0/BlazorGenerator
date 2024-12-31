using BlazorGenerator.DynamicComponents;
using BlazorGenerator.Models;
using BlazorGenerator.Security;
using BlazorGenerator.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator
{
  public static class BlazorGeneratorServices
  {
    public static IServiceCollection AddBlazorGenerator(this IServiceCollection services)
    {
      services.AddTransient<IMainLayout, DynamicMainLayout>();
      services.AddFluentUIComponents();

      services.AddSingleton<BlazorGenLogger>();
      services.AddScoped<ProgressService>();
      services.AddScoped<LockUIService>();
      
      services.AddScoped<ISecurity, NullSecurity>();
      services.AddScoped<BlazorGeneratorSecurityService>();
      services.AddScoped<IHelpService, EmptyHelpService>();
      services.AddScoped<UIServices>();

      return services;
    }
  }
}
