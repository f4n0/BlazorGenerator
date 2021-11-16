using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Eos.Blazor.Generator.Utils
{
  static class AttributesUtils
  {
    internal static IEnumerable<(PropertyInfo Property, TAttribute Attribute)> GetPropertiesWithAttribute<TAttribute>(object obj) where TAttribute : Attribute
    {
      return (from p in obj.GetType().GetProperties()
              let attr = p.GetCustomAttributes(typeof(TAttribute), true)
              where attr.Length == 1
              select new { Prop = p, Attribute = attr.First() as TAttribute })
              .Select(data => (data.Prop, data.Attribute));
    }

    internal static IEnumerable<(MethodInfo Method, TAttribute Attribute)> GetMethodsWithAttribute<TAttribute>(object obj) where TAttribute : Attribute
    {
      return (from p in obj.GetType().GetMethods()
              let attr = p.GetCustomAttributes(typeof(TAttribute), true)
              where attr.Length == 1
              select new { Prop = p, Attribute = attr.First() as TAttribute })
              .Select(data => (data.Prop, data.Attribute));
    }

    internal static IEnumerable<(Type Type, TAttribute Attribute)> GetModelsWithAttribute<TAttribute>() where TAttribute : Attribute
    {
      return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(t => t.IsDefined(typeof(TAttribute), true)))
        .Select(t => (t, (t.GetCustomAttribute(typeof(TAttribute), true) as TAttribute)));
    }

    internal static IEnumerable<TAttribute> GetModelsWithAttribute<TAttribute>(object obj) where TAttribute : Attribute
    {
      return obj.GetType().GetCustomAttributes(typeof(TAttribute), true).Select(t => t as TAttribute);
    }
  }
}
