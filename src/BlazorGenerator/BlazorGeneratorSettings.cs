using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator
{
  public class BlazorGeneratorSettings
  {
    private static BlazorGeneratorSettings? _instance;
    public static BlazorGeneratorSettings Instance => _instance ??= new BlazorGeneratorSettings();

    public string ApplicationName { get; set; } = "BlazorGenerator App";
    public OfficeColor BaseColor { get; set; } = OfficeColor.Default;

    public string UnauthorizedRoute { get; set; } = "/Unauthorized";
    public string LoginRoute { get; set; } = "/Login";

  }
}
