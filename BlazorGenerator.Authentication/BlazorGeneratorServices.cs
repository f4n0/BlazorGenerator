using BlazorGenerator.DynamicComponents;
using BlazorGenerator.Security;
using BlazorGenerator.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator.Authentication
{
  public static class BlazorGeneratorServices
  {
    public static IServiceCollection UseBlazorGeneratorAuthentication(this IServiceCollection services)
    {      
      services.AddTransient<IMainLayout, AuthMainLayout>();
      return services;
    }
  }
}
