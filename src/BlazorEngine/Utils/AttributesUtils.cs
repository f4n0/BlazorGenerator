using System.Collections.Concurrent;
using System.Reflection;

namespace BlazorEngine.Utils;

internal static class AttributesUtils
{
  internal static IEnumerable<(PropertyInfo Property, TAttribute Attribute)>
    GetPropertiesWithAttribute<TAttribute>(object obj)
    where TAttribute : Attribute
  {
    var type = obj.GetType();

    return Cache<TAttribute>.Properties.GetOrAdd(type, static t =>
    {
      const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;

      var props = t.GetProperties(flags);
      var list = new List<(PropertyInfo, TAttribute)>(props.Length);

      foreach (var p in props)
      {
        if (p.GetIndexParameters().Length != 0) // skip indexers
          continue;

        var attr = p.GetCustomAttribute<TAttribute>(true);
        if (attr != null)
          list.Add((p, attr));
      }

      return list.ToArray();
    });
  }

  internal static IEnumerable<(MethodInfo Method, TAttribute Attribute)> GetMethodsWithAttribute<TAttribute>(object obj)
    where TAttribute : Attribute
  {
    var type = obj.GetType();

    return Cache<TAttribute>.Methods.GetOrAdd(type, static t =>
    {
      const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

      var methods = t.GetMethods(flags);
      var list = new List<(MethodInfo, TAttribute)>();

      foreach (var m in methods)
      {
        if (m.IsSpecialName) // filters get_XXX/set_XXX/add_XXX/remove_XXX, operators, etc.
          continue;

        var attr = m.GetCustomAttribute<TAttribute>(true);
        if (attr != null)
          list.Add((m, attr));
      }

      return list.ToArray();
    });
  }

  internal static IEnumerable<(Type Type, TAttribute Attribute)> GetModelsWithAttribute<TAttribute>()
    where TAttribute : Attribute
  {
    return Cache<TAttribute>.Models.Value;
  }

  internal static IEnumerable<TAttribute> GetModelsWithAttribute<TAttribute>(object obj)
    where TAttribute : Attribute
  {
    return obj.GetType().GetCustomAttributes<TAttribute>(true);
  }

  private static class Cache<TAttribute> where TAttribute : Attribute
  {
    internal static readonly ConcurrentDictionary<Type, (PropertyInfo Property, TAttribute Attribute)[]> Properties =
      new();

    internal static readonly ConcurrentDictionary<Type, (MethodInfo Method, TAttribute Attribute)[]> Methods = new();

    internal static readonly Lazy<(Type Type, TAttribute Attribute)[]> Models =
      new(BuildModels, LazyThreadSafetyMode.ExecutionAndPublication);

    private static (Type Type, TAttribute Attribute)[] BuildModels()
    {
      var results = new List<(Type, TAttribute)>();
      var skipPrefixes = new[]
        { "System.", "Microsoft.", "netstandard", "mscorlib", "WindowsBase", "Presentation", "Newtonsoft." };

      foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        var name = assembly.GetName().Name;
        if (name == null)
          continue;
        if (skipPrefixes.Any(prefix => name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
          continue;

        foreach (var type in GetLoadableTypes(assembly))
        {
          var attr = type.GetCustomAttribute<TAttribute>(true);
          if (attr != null)
            results.Add((type, attr));
        }
      }

      return results.ToArray();
    }

    private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
    {
      try
      {
        return assembly.GetTypes();
      }
      catch (ReflectionTypeLoadException ex)
      {
        return ex.Types.Where(t => t != null)!;
      }
      catch
      {
        return Array.Empty<Type>();
      }
    }
  }
}