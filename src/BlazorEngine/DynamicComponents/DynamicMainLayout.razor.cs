using BlazorEngine.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine.DynamicComponents;

public partial class DynamicMainLayout
{
  private bool _navigationExpanded = true;
  private bool _isMobile;

  [Inject] private IThemeService ThemeService { get; set; } = default!;

  [Inject] public IHelpService? HelpService { get; set; }

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (firstRender)
    {
      var stored = await ThemeService.GetThemeSettingsAsync();
      var configured = BlazorEngineSettings.Instance.Theme;
      await ThemeService.SetThemeAsync(stored is null ? configured : configured with { Mode = stored.Mode });
    }
    await base.OnAfterRenderAsync(firstRender);
  }

  private Task SwitchDarkLightTheme() => ThemeService.SwitchThemeAsync();
}
