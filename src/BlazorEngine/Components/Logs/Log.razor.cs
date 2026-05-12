using BlazorEngine.Components.Base;
using BlazorEngine.Enum;
using BlazorEngine.Models;
using BlazorEngine.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorEngine.Components.Logs;

public partial class Log : BlazorEngineComponentBase
{
  private FluentButton? _clearLogButton;
  private FluentButton? _logButton;

  private ElementReference DivRef;
  private bool Hidden { get; set; } = true;
  private bool _focusClearAction;
  private bool _restoreLauncherFocus;

  [Inject] private IKeyCodeService? KeyCodeService { get; set; }

  private void OnDismiss(DialogEventArgs args)
  {
    if (args.Reason is not null)
      CloseLog();
  }

  private static Color ConvertToColor(LogType logType)
  {
    return logType switch
    {
      LogType.Error => Color.Error,
      LogType.Info => Color.Neutral,
      LogType.Warning => Color.Warning,
      _ => Color.Neutral
    };
  }

  protected override void OnInitialized()
  {
    UIServices.Logger.OnChange += UpdateLog;
    KeyCodeService!.RegisterListener(OnKeyDownAsync);
  }

  public async Task OnKeyDownAsync(FluentKeyCodeEventArgs args)
  {
    if (args.AltKey && args.Key == KeyCode.KeyL)
      OpenLog();

    await Task.CompletedTask;
  }

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (_focusClearAction && !Hidden && _clearLogButton != null)
    {
      _focusClearAction = false;
      await _clearLogButton.Element.FocusAsync();
    }

    if (_restoreLauncherFocus && Hidden && _logButton != null)
    {
      _restoreLauncherFocus = false;
      await _logButton.Element.FocusAsync();
    }

    if (!Hidden)
      await JSRuntime.InvokeVoidAsync("scrollToEnd", DivRef);

    await base.OnAfterRenderAsync(firstRender);
  }

  private void UpdateLog()
  {
    _ = InvokeAsync(() => { StateHasChanged(); });
  }

  private void OpenLog()
  {
    _focusClearAction = true;
    Hidden = false;
  }

  private void CloseLog()
  {
    Hidden = true;
    _restoreLauncherFocus = true;
  }

  private void ClearLog()
  {
    UIServices.Logger.Logs = new CircularLogBuffer(BlazorEngineLogger.MaxLogEntries);
    InvokeAsync(() => StateHasChanged());
  }

  public override void InternalDispose()
  {
    UIServices.Logger.OnChange -= UpdateLog;
    KeyCodeService?.UnregisterListener(OnKeyDownAsync);
    base.InternalDispose();
  }

  public override ValueTask InternalDisposeAsync()
  {
    UIServices.Logger.OnChange -= UpdateLog;
    KeyCodeService?.UnregisterListener(OnKeyDownAsync);
    return base.InternalDisposeAsync();
  }
}