using BlazorGenerator.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator.Authentication
{
  public partial class AuthMainLayout
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

    private void SwitchDarkLightTheme()
    {
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
      Theme!.Mode = Theme.Mode == DesignThemeModes.Light ? DesignThemeModes.Dark : DesignThemeModes.Light;
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
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
