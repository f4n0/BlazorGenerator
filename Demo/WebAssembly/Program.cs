using BlazorEngine;
using BlazorEngine.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using TestShared.Services;
using WebAssembly;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IHelpService, HelpService>();

builder.Services.AddBlazorEngine();
BlazorEngineSettings.Instance.ApplicationName = "BlazorEngine Demo App";
BlazorEngineSettings.Instance.BaseColor = OfficeColor.Access;
BlazorEngineSettings.Instance.ShowBackgroundTasks = true;
await builder.Build().RunAsync();