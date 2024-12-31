using BlazorGenerator.DynamicComponents;
using Microsoft.Extensions.DependencyInjection;

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
