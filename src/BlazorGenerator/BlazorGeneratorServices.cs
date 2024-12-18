﻿using BlazorGenerator.DynamicComponents;
using BlazorGenerator.Models;
using BlazorGenerator.Security;
using BlazorGenerator.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;

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
      services.AddTransient<IHelpService, EmptyHelpService>();

      services.AddSingleton<UIServices>(serviceProvider => new UIServices(
              (serviceProvider.GetService(typeof(BlazorGenLogger)) as BlazorGenLogger)!,
              null!,
              (serviceProvider.CreateScope().ServiceProvider.GetService<ProgressService>())!,
              null!,
              (serviceProvider.CreateScope().ServiceProvider.GetService<LockUIService>())!
           ));

      return services;
    }
  }
}
