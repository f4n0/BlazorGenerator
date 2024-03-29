using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebAssembly;
using BlazorGenerator;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddBlazorGenerator();
BlazorGeneratorSettings.Instance.ApplicationName = "BlazorGenerator Demo App";
BlazorGeneratorSettings.Instance.BaseColor = Microsoft.FluentUI.AspNetCore.Components.OfficeColor.Access;

await builder.Build().RunAsync();
