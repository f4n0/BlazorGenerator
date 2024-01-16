using BlazorGenerator.Security;
using BlazorGenerator.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using System;

namespace BlazorGenerator
{
    public static class BlazorGeneratorServices
    {
        public static IServiceCollection AddBlazorGenerator(this IServiceCollection services)
        {
            services.AddFluentUIComponents();

            services.AddSingleton<BlazorGenLogger>();
            services.AddScoped<ProgressService>();

            services.AddScoped<ISecurity, NullSecurity>();
            services.AddTransient<BlazorGeneratorSecurity>();

            services.AddSingleton<UIServices>(serviceProvider => new UIServices(

              //serviceProvider.CreateScope().ServiceProvider.GetService(typeof(IPageProgressService)) as IPageProgressService,
              //serviceProvider.CreateScope().ServiceProvider.GetService(typeof(IMessageService)) as IMessageService,
              serviceProvider.GetService(typeof(BlazorGenLogger)) as BlazorGenLogger,
              null,
              serviceProvider.CreateScope().ServiceProvider.GetService<ProgressService>()
           ));
            
            return services;
        }

    }
}
