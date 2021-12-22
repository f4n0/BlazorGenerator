using BlazorGenerator.Attributes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Services
{
  public class DynamicRoute : RouteView
  {
    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields", Justification = "<Pending>")]
    protected override void Render(RenderTreeBuilder builder)
    {
      var pageLayoutType = RouteData.PageType.GetCustomAttribute<LayoutAttribute>()?.LayoutType
          ?? DefaultLayout;

      var fiRenderPageWithParametersDelegate = typeof(RouteView)
        .GetField("_renderPageWithParametersDelegate", BindingFlags.Instance | BindingFlags.NonPublic);
      var _renderPageWithParametersDelegate = fiRenderPageWithParametersDelegate.GetValue(this);

      builder.OpenComponent<LayoutView>(0);
      builder.AddAttribute(1, nameof(LayoutView.Layout), pageLayoutType);
      builder.AddAttribute(2, nameof(LayoutView.ChildContent), _renderPageWithParametersDelegate);
      builder.CloseComponent();
    }
  }
}
