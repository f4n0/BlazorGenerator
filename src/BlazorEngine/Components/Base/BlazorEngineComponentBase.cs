using BlazorEngine.Actions;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine.Components.Base;

public partial class BlazorEngineComponentBase : IAsyncDisposable, IDisposable
{
  private CancellationTokenSource? _cancellationTokenSource;
  private Func<FluentKeyCodeEventArgs, Task>? _keyDownHandler;
  private bool _keyDownRegistered;

  protected virtual bool UsesPageLifecycle => false;
  public virtual string Title => GetType().Name;

  protected virtual IReadOnlyList<PageActionDefinition> CreatePageActions() => [];

  public virtual bool ShowButtons { get; set; } = true;
  public virtual bool ShowActions { get; set; } = true;
  public CancellationToken ComponentDetached => (_cancellationTokenSource ??= new CancellationTokenSource()).Token;


  public async ValueTask DisposeAsync()
  {
    await InternalDisposeAsync();
    GC.SuppressFinalize(this);
  }

  public void Dispose()
  {
    InternalDispose();
  }


  protected virtual async Task LoadVisibleFields()
  {
    await Task.CompletedTask;
  }

  protected virtual async Task LoadData()
  {
    await Task.CompletedTask;
  }

  protected override async Task OnParametersSetAsync()
  {
    if (UsesPageLifecycle)
    {
      await LoadVisibleFields();
      if (!_keyDownRegistered)
      {
        _keyDownHandler ??= OnKeyDownAsync;
        UIServices.KeyCodeService.RegisterListener(_keyDownHandler);
        _keyDownRegistered = true;
      }
    }

    await base.OnParametersSetAsync();
  }

  protected override async Task OnInitializedAsync()
  {
    if (UsesPageLifecycle) await LoadData();
    await base.OnInitializedAsync();
  }

  internal async Task<bool> OnRefreshAsync()
  {
    await LoadData();

    await InvokeAsync(() => StateHasChanged());

    return true;
  }

  private async Task OnKeyDownAsync(FluentKeyCodeEventArgs args)
  {
    if (args.Key == KeyCode.Function5) await OnRefreshAsync();
  }


  public virtual ValueTask InternalDisposeAsync()
  {
    if (_cancellationTokenSource != null)
    {
      _cancellationTokenSource.Cancel(false);
      _cancellationTokenSource.Dispose();
      _cancellationTokenSource = null;
    }

    if (_keyDownHandler != null && _keyDownRegistered)
    {
      UIServices.KeyCodeService.UnregisterListener(_keyDownHandler);
      _keyDownHandler = null;
      _keyDownRegistered = false;
    }

    GC.SuppressFinalize(this);
    return ValueTask.CompletedTask;
  }

  public virtual void InternalDispose()
  {
    if (_cancellationTokenSource != null)
    {
      _cancellationTokenSource.Cancel();
      _cancellationTokenSource.Dispose();
      _cancellationTokenSource = null;
    }

    if (_keyDownHandler != null && _keyDownRegistered)
    {
      UIServices.KeyCodeService.UnregisterListener(_keyDownHandler);
      _keyDownHandler = null;
      _keyDownRegistered = false;
    }

    GC.SuppressFinalize(this);
  }
}
