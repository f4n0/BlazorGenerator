using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TestShared.Services;
using WebAssembly;
using BlazorEngine;
using BlazorEngine.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IHelpService, HelpService>();

builder.Services.AddBlazorEngine();
BlazorEngineSettings.Instance.ApplicationName = "BlazorEngine Demo App";
BlazorEngineSettings.Instance.BaseColor = Microsoft.FluentUI.AspNetCore.Components.OfficeColor.Access;

await builder.Build().RunAsync();
