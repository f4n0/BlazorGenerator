using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Infrastructure
{
  public partial class DynamicErrorHandler
  {
    [Parameter]
    public ErrorBoundary errorBoundary { get; set; }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
      try
      {
        ErrorBoundaryBase tmp = errorBoundary;
        var test = (Exception)GetInstanceField(tmp.GetType().BaseType, tmp, "CurrentException");
        ConsoleLog("Start Error Logging");
        ConsoleLog(test.Message);
        ConsoleLog(test.StackTrace);
        ConsoleLog("End Error Logging");
        string error = $"""
<div class="rounded-3">
      <div class="container-fluid">
        <h3 class="fw-bold">An Error Occurred</h3>
        <p class="col-md-8 text-muted" style="text-align: left !important;">{test.Message}</p>
      </div>
    </div>
""";
        MessageService.Error((MarkupString)error, "Error", option=> option.ShowMessageIcon = false);
      }
      catch
      {


        MessageService.Error("An Unexpected error occurred!", "Error");
      }
      //errorBoundary.Recover();
      return base.OnAfterRenderAsync(firstRender);
    }

    internal static object GetInstanceField(Type type, object instance, string fieldName)
    {
      BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
          | BindingFlags.Static;
      var field = type.GetProperty(fieldName, bindFlags);
      return field.GetValue(instance);
    }

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    public async void ConsoleLog(string message)
    {
      await JSRuntime.InvokeVoidAsync("console.log", message);
    }
  }
}
