using BlazorEngine.Components.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine.Components.Logs;

public partial class Log : BlazorEngineComponentBase
{
  private FluentButton? _logButton;
  private bool _isOpen;

  [Inject] private IKeyCodeService KeyCodeService { get; set; } = default!;

  protected override void OnInitialized()
  {
    KeyCodeService.RegisterListener(OnKeyDownAsync);
    base.OnInitialized();
  }

  public async Task OnKeyDownAsync(FluentKeyCodeEventArgs args)
  {
    if (args.AltKey && args.Key == KeyCode.KeyL) await OpenLog();
  }

  private async Task OpenLog()
  {
    if (_isOpen) return;
    _isOpen = true;
    var options = BlazorEngine.Services.UIServices.CreateDialogOptions("60vw", "60vh", Captions.Instance.Log);
    options.Parameters[nameof(LogPanel.Logger)] = UIServices.Logger;
    options.Footer.PrimaryAction.Visible = false;
    options.Footer.SecondaryAction.Visible = false;
    try
    {
      await UIServices.DialogService.ShowDialogAsync<LogPanel>(options);
    }
    finally
    {
      _isOpen = false;
      if (_logButton != null) await _logButton.Element.FocusAsync();
    }
  }

  public override void InternalDispose()
  {
    KeyCodeService.UnregisterListener(OnKeyDownAsync);
    base.InternalDispose();
  }

  public override ValueTask InternalDisposeAsync()
  {
    KeyCodeService.UnregisterListener(OnKeyDownAsync);
    return base.InternalDisposeAsync();
  }
}
