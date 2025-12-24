using BlazorEngine.Layouts;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine.Components.Base
{
  public partial class BlazorEngineComponentBase : IAsyncDisposable, IDisposable
  {
    public virtual string Title => this.GetType().Name;

    public virtual bool ShowButtons { get; set; } = true;
    public virtual bool ShowActions { get; set; } = true;


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
      if (UseBlazorEngineLayouts())
      {
        await LoadData();
      }
      await base.OnInitializedAsync();
    }

    private bool? _useBlazorEngineLayoutsCached;
    
    private bool UseBlazorEngineLayouts()
    {
      if (_useBlazorEngineLayoutsCached.HasValue)
        return _useBlazorEngineLayoutsCached.Value;

      var loadingBaseType = this.GetType().BaseType;
      if (loadingBaseType == null)
      {
        _useBlazorEngineLayoutsCached = false;
        return false;
      }

      bool ret = loadingBaseType.IsGenericType && (
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

      StateHasChanged();

      return true;
    }

    private async Task OnKeyDownAsync(FluentKeyCodeEventArgs args)
    {
      if (args.Key == KeyCode.Function5)
      {
        await OnRefreshAsync();        
      }
    }

    private CancellationTokenSource? _cancellationTokenSource;
    public CancellationToken ComponentDetached => (_cancellationTokenSource ??= new()).Token;


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


    public async ValueTask DisposeAsync()
    {
      await InternalDisposeAsync();
      GC.SuppressFinalize(this);
    }
    public void Dispose() => InternalDispose();
  }
}
