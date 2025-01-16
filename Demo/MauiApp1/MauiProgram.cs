using Microsoft.Extensions.Logging;
using BlazorEngine;
using BlazorEngine.Services;
using TestShared.Services;

namespace MauiApp1
{
  public static class MauiProgram
  {
    public static MauiApp CreateMauiApp()
    {
      var builder = MauiApp.CreateBuilder();
      builder
        .UseMauiApp<App>()
        .ConfigureFonts(fonts => fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"));

      builder.Services.AddMauiBlazorWebView();
      builder.Services.AddBlazorGenerator();
      
      builder.Services.AddScoped<IHelpService, HelpService>();
#if DEBUG
      builder.Services.AddBlazorWebViewDeveloperTools();
      builder.Logging.AddDebug();
#endif

      var app = builder.Build();

      return app;
    }
  }
}
