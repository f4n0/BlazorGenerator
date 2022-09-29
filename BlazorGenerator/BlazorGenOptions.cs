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

    public Theme Theme { get; set; } = new Theme()
    {
      LuminanceThreshold = 170,
      BarOptions = new()
      {
        HorizontalHeight = "64px",
        VerticalBrandHeight = "64px",
        LightColors = new()
        {
          ItemColorOptions = new()
          {
            ActiveBackgroundColor = "#dedede",
            ActiveColor = "#000000",
            HoverBackgroundColor = "#dedede",
            HoverColor = "#000000",

          },
        }        
      },
      ColorOptions = new()
      {
        Primary = "#1e88e5",
        Secondary = "#382F2D",
      },
      BackgroundOptions = new()
      {
        Primary = "#1e88e5",
        Secondary = "#382F2D",
      },
      TextColorOptions = new()
      {
        Primary = "#1e88e5",
        Secondary = "#382F2D",
      },
      InputOptions = new()
      {
        CheckColor = "#1e88e5",
      }      

    };

    public IServiceProvider Services => serviceProvider;
  }
}
