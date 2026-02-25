using BlazorEngine.Layouts;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine.Components.Base;

public partial class BlazorEngineComponentBase : IAsyncDisposable, IDisposable
{
  private CancellationTokenSource? _cancellationTokenSource;

  private bool? _useBlazorEngineLayoutsCached;
  public virtual string Title => GetType().Name;

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
    if (UseBlazorEngineLayouts())
    {
      await LoadVisibleFields();
      UIServices.KeyCodeService.RegisterListener(OnKeyDownAsync);
    }

    await base.OnParametersSetAsync();
  }

  protected override async Task OnInitializedAsync()
  {
    if (UseBlazorEngineLayouts()) await LoadData();
    await base.OnInitializedAsync();
  }

  private bool UseBlazorEngineLayouts()
  {
    if (_useBlazorEngineLayoutsCached.HasValue)
      return _useBlazorEngineLayoutsCached.Value;

    var loadingBaseType = GetType().BaseType;
    if (loadingBaseType == null)
    {
      _useBlazorEngineLayoutsCached = false;
      return false;
    }

    var ret = loadingBaseType.IsGenericType && (
      loadingBaseType.GetGenericTypeDefinition() == typeof(CardPage<>) ||
      loadingBaseType.GetGenericTypeDefinition() == typeof(ListPage<>) ||
      loadingBaseType.GetGenericTypeDefinition() == typeof(Worksheet<,>) ||
      loadingBaseType.GetGenericTypeDefinition() == typeof(TwoList<,>));

    _useBlazorEngineLayoutsCached = ret;
    return ret;
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

    UIServices.KeyCodeService.UnregisterListener(OnKeyDownAsync);
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

    UIServices.KeyCodeService.UnregisterListener(OnKeyDownAsync);
    GC.SuppressFinalize(this);
  }
}