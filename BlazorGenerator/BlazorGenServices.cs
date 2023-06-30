﻿using BlazorGenerator.Security;
using BlazorGenerator.Services;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.Extensions.DependencyInjection;
using System;

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
      })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

      services.AddSingleton<BlazorGenLogger>();


      configureOptions ??= _ => { };
      services.AddSingleton(configureOptions);
      services.AddSingleton<BlazorGenOptions>();

      services.AddScoped<ISecurity, NullSecurity>();
      services.AddTransient<BlazorGenSecurity>();

      services.AddSingleton<UIServices>(serviceProvider => new UIServices(
        
        serviceProvider.CreateScope().ServiceProvider.GetService(typeof(IPageProgressService)) as IPageProgressService,
        serviceProvider.CreateScope().ServiceProvider.GetService(typeof(IMessageService)) as IMessageService,
        serviceProvider.GetService(typeof(BlazorGenLogger)) as BlazorGenLogger,
        serviceProvider.CreateScope().ServiceProvider.GetService(typeof(INotificationService)) as INotificationService,
        serviceProvider.CreateScope().ServiceProvider.GetService(typeof(IModalService)) as IModalService
     ));

      return services;
    }

  }
}
