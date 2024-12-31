using BlazorGenerator;
using BlazorGenerator.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using TestShared.Services;
using WebAssembly;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IHelpService, HelpService>();

builder.Services.AddBlazorGenerator();
BlazorGeneratorSettings.Instance.ApplicationName = "BlazorGenerator Demo App";
BlazorGeneratorSettings.Instance.BaseColor = Microsoft.FluentUI.AspNetCore.Components.OfficeColor.Access;

await builder.Build().RunAsync();
