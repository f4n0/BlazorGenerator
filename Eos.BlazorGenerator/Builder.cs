using Eos.BlazorGenerator.Attributes;
using Eos.BlazorGenerator.Enum;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eos.BlazorGenerator
{
  public class Builder
  {

    public static RenderFragment buildMenu(AppDomain domain = null)
    {
      domain = AppDomain.CurrentDomain;
      var menuItems = GetTypesWith<AddToMenu>(domain, false);

      string HtmlItem = "<li class=\"nav-item px-3\"><a class=\"nav-link\" href=\"%model%\"><span class=\"oi oi-home\" aria-hidden=\"true\"></span> %caption%</a></li>";

      return new RenderFragment(rf =>
      {
        foreach (var item in menuItems)
        {
          rf.AddMarkupContent(1, HtmlItem.Replace("%model%", item.Key.Route).Replace("%caption%", item.Key.Title));
        }
      });
    }

    private static IEnumerable<KeyValuePair<TAttribute, object>> GetTypesWith<TAttribute>(AppDomain domain, bool inherit) where TAttribute : Attribute
    {
      List<KeyValuePair<TAttribute, object>> output = new List<KeyValuePair<TAttribute, object>>();

      var assemblies = domain.GetAssemblies();

      foreach (var assembly in assemblies)
      {
        var assembly_types = assembly.GetTypes();

        foreach (var type in assembly_types)
        {
          if (type.IsDefined(typeof(TAttribute), inherit))
            type.GetCustomAttributes(typeof(TAttribute), inherit).ToList().ForEach(el =>
            {
              output.Add(new KeyValuePair<TAttribute, object>((el as TAttribute), type));

            });
        }
      }

      return output;
    }

    internal static RenderFragment buildPage(PageTypes pageTypes, string v)
    {
      return new RenderFragment(rf =>
      {
        rf.OpenElement(0, "div");
        rf.AddContent(1,v);
        rf.CloseElement();
      });
    }
  }
}
