using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine
{
  public class BlazorEngineSettings
  {
    private static BlazorEngineSettings? _instance;
    public static BlazorEngineSettings Instance => _instance ??= new BlazorEngineSettings();

    public string ApplicationName { get; set; } = "BlazorEngine App";

    public string UnauthorizedRoute { get; set; } = "/Unauthorized";
    public string LoginRoute { get; set; } = "/Login";

    public bool ShowHelpButton { get; set; } = false;
    public bool ShowLogButton { get; set; } = true;
    public bool ShowBackgroundTasks { get; set; } = false;
  }
}
