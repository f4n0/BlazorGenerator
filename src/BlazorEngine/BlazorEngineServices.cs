using BlazorEngine.DynamicComponents;
using BlazorEngine.Models;
using BlazorEngine.Security;
using BlazorEngine.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine
{
  public static class BlazorEngineServices
  {
    public static IServiceCollection AddBlazorGenerator(this IServiceCollection services)
    {
      services.AddTransient<IMainLayout, DynamicMainLayout>();
      services.AddFluentUIComponents();

      services.AddSingleton<BlazorEngineLogger>();
      services.AddScoped<ProgressService>();
      services.AddScoped<LockUIService>();
      
      services.AddScoped<ISecurity, NullSecurity>();
      services.AddScoped<BlazorEngineSecurityService>();
      services.AddScoped<IHelpService, EmptyHelpService>();
      services.AddScoped<UIServices>();

      return services;
    }
  }
}
