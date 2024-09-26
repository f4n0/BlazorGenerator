using BlazorGenerator.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator.DynamicComponents
{
  public partial class DynamicMainLayout : IMainLayout
  {
    FluentDesignTheme? Theme { get; set; }

    [Inject]
    public UIServices? UIServices { get; set; }
    [Inject]
    public IDialogService? DialogService { get; set; }
    [Inject]
    public ProgressService? ProgressService { get; set; }
    [Inject]
    public IKeyCodeService? KeyCodeService { get; set; }
    [Inject]
    public LockUIService? LockUIService { get; set; }
    [Inject]
    public IHelpService? HelpService { get; set; }

    private void SwitchDarkLightTheme()
    {
      Theme!.Mode = Theme.Mode == DesignThemeModes.Light ? DesignThemeModes.Dark : DesignThemeModes.Light;
    }

    protected override Task OnParametersSetAsync()
    {
      UIServices!.DialogService = DialogService!;
      UIServices!.ProgressService = ProgressService!;
      UIServices.KeyCodeService = KeyCodeService!;
      UIServices.LockService = LockUIService!;
      return base.OnParametersSetAsync();
    }
  }
}
