using BlazorEngine.Components.Base;
using BlazorEngine.Enum;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine.Components.Logs
{
  public partial class Log : BlazorEngineComponentBase, IDisposable, IAsyncDisposable
  {
    private FluentDialog? _myFluentDialog;
    private bool Hidden { get; set; } = true;

    [Inject]
    private IKeyCodeService? KeyCodeService { get; set; }

    private void OnDismiss(DialogEventArgs args)
    {
      if (args.Reason is not null && args.Reason == "dismiss")
      {
        _myFluentDialog!.Hide();
      }
    }

    private static Color ConvertToColor(LogType logType)
    {
      return logType switch
      {
        LogType.Error => Color.Error,
        LogType.Info => Color.Neutral,
        LogType.Warning => Color.Warning,
        _ => Color.Neutral,
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
      {
        OnOpen();
      }

      await Task.CompletedTask;
    }

    protected override void OnAfterRender(bool firstRender)
    {
      if (firstRender)
        _myFluentDialog!.Hide();
    }

    private void UpdateLog()
    {
      _ = InvokeAsync(() => StateHasChanged());
    }

    public new void Dispose()
    {
      UIServices.Logger.OnChange -= UpdateLog;
      GC.SuppressFinalize(this);
      base.Dispose();
    }

    private void OnOpen()
    {
      _myFluentDialog!.Show();
    }

    public new ValueTask DisposeAsync()
    {
      KeyCodeService!.UnregisterListener(OnKeyDownAsync);
      GC.SuppressFinalize(this);
      return base.DisposeAsync();
    }

    private void ClearLog()
    {
      UIServices.Logger.Logs.Clear();
      StateHasChanged();
    }
  }
}
