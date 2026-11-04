using BlazorEngine;
using BlazorEngine.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using TestShared.Services;
using TestShared;
using WebAssembly;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IHelpService, HelpService>();

builder.Services.AddBlazorEngine(DemoRegistration.Configure);
BlazorEngineSettings.Instance.ApplicationName = "BlazorEngine Demo App";
BlazorEngineSettings.Instance.Theme = new ThemeSettings()
{
  Color = "#C17676"
};
BlazorEngineSettings.Instance.ShowBackgroundTasks = true;
await builder.Build().RunAsync();
