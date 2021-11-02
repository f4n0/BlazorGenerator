using Eos.BlazorGenerator.Attributes;
using Eos.BlazorGenerator.Enum;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Reflection;

namespace Eos.BlazorGenerator
{
  public class EosDynamicRoute : RouteView
  {
    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    protected override void Render(RenderTreeBuilder builder)
    {
      var pageLayoutType = RouteData.PageType.GetCustomAttribute<LayoutAttribute>()?.LayoutType
          ?? DefaultLayout;
      string selectedCSS = string.Empty;
      var attr = RouteData.PageType.GetCustomAttribute<PageTypeAttribute>();
      if (attr != null)
      {
        //switch (attr.Type)
        //{
        var x = typeof(RouteView); 
        var fiRenderPageWithParametersDelegate = typeof(RouteView)
        .GetField("_renderPageWithParametersDelegate", BindingFlags.Instance | BindingFlags.NonPublic);
        var _renderPageWithParametersDelegate = fiRenderPageWithParametersDelegate.GetValue(this);

        builder.OpenComponent<LayoutView>(0);
        builder.AddAttribute(1, nameof(LayoutView.Layout), pageLayoutType);
        builder.AddAttribute(2, nameof(LayoutView.ChildContent), _renderPageWithParametersDelegate);
        builder.CloseComponent();
        //case PageTypes.ListPage:
        //  var method = RouteData.PageType.GetMethod("GetDataJSON");
        //  var data = method.Invoke(null, null);
        //  builder.OpenElement(3, "pre");
        //  builder.AddContent(3, data);
        //  builder.CloseElement();
        //  break;
        //case PageTypes.CardPage:
        //  break;
        //default:
        //  break;
        //}
      }
      else
      {

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
}

