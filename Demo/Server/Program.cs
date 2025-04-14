using TestShared.Services;
using BlazorEngine;
using BlazorEngine.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazorEngine(); //.UseBlazorGeneratorAuthentication();

BlazorEngineSettings.Instance.ApplicationName = "BlazorEngine Demo App";
BlazorEngineSettings.Instance.BaseColor = Microsoft.FluentUI.AspNetCore.Components.OfficeColor.Access;
BlazorEngineSettings.Instance.ShowHelpButton = true;
BlazorEngineSettings.Instance.ShowBackgroundTasks = true;

builder.Services.AddScoped<IHelpService, HelpService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDeveloperExceptionPage();
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();