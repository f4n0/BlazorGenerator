using System.Reflection;

namespace BlazorGenerator.Utils
{
  static class AttributesUtils
  {
    internal static IEnumerable<(PropertyInfo Property, TAttribute Attribute)> GetPropertiesWithAttribute<TAttribute>(object obj) where TAttribute : Attribute
    {
      return (from p in obj.GetType().GetProperties()
              let attr = p.GetCustomAttributes(typeof(TAttribute), true)
              where attr.Length == 1
              select new { Prop = p, Attribute = attr[0] as TAttribute })
              .Select(data => (data.Prop, data.Attribute));
    }

    internal static IEnumerable<(MethodInfo Method, TAttribute Attribute)> GetMethodsWithAttribute<TAttribute>(object obj) where TAttribute : Attribute
    {
      return (from p in obj.GetType().GetMethods()
              let attr = p.GetCustomAttributes(typeof(TAttribute), true)
              where attr.Length == 1
              select new { Prop = p, Attribute = attr[0] as TAttribute })
              .Select(data => (data.Prop, data.Attribute));
    }

    internal static IEnumerable<(Type Type, TAttribute Attribute)> GetModelsWithAttribute<TAttribute>() where TAttribute : Attribute
    {
#pragma warning disable CS8619 // In realtà è gestito
      return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a =>
      {
        try
        {
          return a.GetTypes().Where(t => t.IsDefined(typeof(TAttribute), true));
        }
        catch
        {
          //null
          return Enumerable.Empty<Type>();
        }
      }).Where(t => t != null)
        .Select(t => (t, t.GetCustomAttribute(typeof(TAttribute), true) as TAttribute));
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }

    internal static IEnumerable<TAttribute> GetModelsWithAttribute<TAttribute>(object obj) where TAttribute : Attribute
    {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
      return obj.GetType().GetCustomAttributes(typeof(TAttribute), true).Select(t => t as TAttribute);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }
  }
}
