using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

#pragma warning disable CA2012

namespace BlazorEngine.DynamicComponents;

public partial class BlazorEngineApp : IDisposable
{
  private CancellationTokenSource? _cancellationTokenSource;

  private ErrorBoundary? _errorBoundary;

  private FluentDialog? _myFluentDialog;
  [Inject] private IJSRuntime? JSRuntime { get; set; }

  [Inject] private NavigationManager? NavManager { get; set; }

  [Parameter] public required Assembly AppAssembly { get; set; }

  [Parameter] public IComponentRenderMode BlazorGenRenderMode { get; set; } = RenderMode.InteractiveAuto;

  [Parameter] public IEnumerable<Assembly>? AdditionalAssemblies { get; set; }

  public CancellationToken ComponentDetached => (_cancellationTokenSource ??= new CancellationTokenSource()).Token;

  public void Dispose()
  {
    if (_cancellationTokenSource != null)
    {
      _cancellationTokenSource.Cancel();
      _cancellationTokenSource.Dispose();
      _cancellationTokenSource = null;
    }

    GC.SuppressFinalize(this);
  }

  private void CreateExceptionDetails(Exception ex)
  {
    var sb = new StringBuilder();
    sb.AppendLine(ex.Message);
    sb.AppendLine(ex.StackTrace);
    if (ex.InnerException != null)
    {
      sb.AppendLine("Inner Exception");
      sb.AppendLine(ex.InnerException.Message);
      sb.AppendLine(ex.InnerException.StackTrace);
    }

    _ = JSRuntime!.InvokeVoidAsync("navigator.clipboard.writeText", ComponentDetached, sb.ToString());
  }
}