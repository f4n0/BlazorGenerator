using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine;

public class BlazorEngineSettings
{
  private static BlazorEngineSettings? _instance;
  public static BlazorEngineSettings Instance => _instance ??= new BlazorEngineSettings();

  public string ApplicationName { get; set; } = "BlazorEngine App";
  public OfficeColor BaseColor { get; set; } = OfficeColor.Default;

  public string UnauthorizedRoute { get; set; } = "/Unauthorized";
  public string LoginRoute { get; set; } = "/Login";

  public bool ShowHelpButton { get; set; } = false;
  public bool ShowLogButton { get; set; } = true;
  public bool ShowBackgroundTasks { get; set; } = false;

  /// <summary>
  ///   When true, enables diagnostic logging of metadata discovery.
  ///   Shows which assemblies were loaded via build-time metadata vs reflection fallback.
  /// </summary>
  public bool EnableDiagnostics { get; set; } = false;

  /// <summary>
  ///   When true, forces all discovery through reflection (ignores build-time metadata).
  ///   Useful for troubleshooting. Default: false.
  /// </summary>
  public bool UseLegacyReflection { get; set; } = false;
}