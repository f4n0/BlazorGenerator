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
    var options = new DialogOptions
    {
      Width = "60vw",
      Height = "60vh",
      Padding = "2rem",
      Parameters = new Dictionary<string, object?> { [nameof(LogPanel.Logger)] = UIServices.Logger }
    };
    options.Header.CloseAction.Visible = false;
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
