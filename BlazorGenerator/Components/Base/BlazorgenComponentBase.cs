using BlazorGenerator.Layouts;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator.Components.Base
{
  public partial class BlazorgenComponentBase : ComponentBase, IDisposable, IAsyncDisposable
  {
    public virtual string Title => this.GetType().Name;

    public virtual bool ShowButtons { get; set; } = true;
    public virtual bool ShowActions { get; set; } = true;

    public void Dispose()
    {
      GC.SuppressFinalize(this);
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
      if (useBlazorGeneratorLayouts())
      {
        await LoadVisibleFields();
        UIServices.KeyCodeService.RegisterListener(OnKeyDownAsync);
      }

      await base.OnParametersSetAsync();
    }


    protected override async Task OnInitializedAsync()
    {
      if (useBlazorGeneratorLayouts())
      {
        await LoadData();
      }
      await base.OnInitializedAsync();
    }

    private bool useBlazorGeneratorLayouts()
    {
      var loadingBaseType = this.GetType().BaseType;

      bool ret = false;

      ret = ret || loadingBaseType.IsGenericType && loadingBaseType.GetGenericTypeDefinition() == typeof(CardPage<>);
      ret = ret || loadingBaseType.IsGenericType && loadingBaseType.GetGenericTypeDefinition() == typeof(ListPage<>);
      ret = ret || loadingBaseType.IsGenericType && loadingBaseType.GetGenericTypeDefinition() == typeof(Worksheet<,>);
      ret = ret || loadingBaseType.IsGenericType && loadingBaseType.GetGenericTypeDefinition() == typeof(TwoList<,>);

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

    public ValueTask DisposeAsync()
    {
      UIServices.KeyCodeService.UnregisterListener(OnKeyDownAsync);
      return ValueTask.CompletedTask;
    }



  }
}
