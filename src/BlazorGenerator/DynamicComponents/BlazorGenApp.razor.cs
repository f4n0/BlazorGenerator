using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Reflection;
using System.Text;
#pragma warning disable CA2012

namespace BlazorGenerator.DynamicComponents
{
  public partial class BlazorGenApp
  {
    [Inject]
    IJSRuntime? JSRuntime { get; set; }

    [Inject]
    NavigationManager? NavManager { get; set; }

    [Parameter]
    public required Assembly AppAssembly { get; set; }
    [Parameter]
    public bool UseDefaultTemplate { get; set; } = false;

    [Parameter]
    public IComponentRenderMode BlazorGenRenderMode { get; set; } = RenderMode.InteractiveAuto;

    private ErrorBoundary? errorBoundary;

    private FluentDialog? _myFluentDialog;

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

      _ = JSRuntime!.InvokeVoidAsync("navigator.clipboard.writeText", sb.ToString());
    }
  }
}
