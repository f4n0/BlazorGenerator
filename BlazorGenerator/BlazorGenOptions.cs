using Blazorise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator
{
  public class BlazorGenOptions
  {
    private readonly IServiceProvider serviceProvider;

    public BlazorGenOptions(IServiceProvider serviceProvider, Action<BlazorGenOptions> configureOptions)
    {
      this.serviceProvider = serviceProvider;
      configureOptions?.Invoke(this);
    }

    public bool ShowBreaddcrumbs { get; set; } = true;

    public bool ShowDarkModeSelector { get; set; } = true;
    public bool UseDarkTheme { get; set; } = false;
    public Theme LightTheme { get; set; } = new();
    public Theme DarkTheme { get; set; } = new()
    {
      BodyOptions = new()
      {
        BackgroundColor = "#444444",
        TextColor = "#ffffff",
      },
      BarOptions = new ThemeBarOptions
      {
        DarkColors = new ThemeBarColorOptions
        {
          BackgroundColor = "#252525",
          GradientBlendPercentage = 10,
        }
      },
      BackgroundOptions = new()
      {
        Body = "#444444"
      },
      Black = "#444444",
      TableOptions = new()
      {
        BackgroundLevel = -500
      },      
    };

    public IServiceProvider Services => serviceProvider;
  }
}
