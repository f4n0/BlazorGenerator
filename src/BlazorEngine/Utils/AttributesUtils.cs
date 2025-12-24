using System.Collections.Concurrent;
using System.Reflection;

namespace BlazorEngine.Utils
{
  static class AttributesUtils
  {
    private static readonly ConcurrentDictionary<(Type ObjectType, Type AttributeType), object> _methodCache = new();
        private static readonly ConcurrentDictionary<(Type ObjectType, Type AttributeType), object> _propertyCache = new();
        private static readonly ConcurrentDictionary<Type, object> _modelCache = new();

        internal static IEnumerable<(PropertyInfo Property, TAttribute Attribute)> GetPropertiesWithAttribute<TAttribute>(object obj) 
            where TAttribute : Attribute
        {
            var key = (obj.GetType(), typeof(TAttribute));

            return (IEnumerable<(PropertyInfo, TAttribute)>)_propertyCache.GetOrAdd(key, _ =>
                (from p in obj.GetType().GetProperties()
                 let attr = p.GetCustomAttributes(typeof(TAttribute), true)
                 where attr.Length == 1
                 select (p, (TAttribute)attr[0])).ToList());
        }

        internal static IEnumerable<(MethodInfo Method, TAttribute Attribute)> GetMethodsWithAttribute<TAttribute>(object obj) 
            where TAttribute : Attribute
        {
            var key = (obj.GetType(), typeof(TAttribute));

            return (IEnumerable<(MethodInfo, TAttribute)>)_methodCache.GetOrAdd(key, _ =>
                (from p in obj.GetType().GetMethods()
                 let attr = p.GetCustomAttributes(typeof(TAttribute), true)
                 where attr.Length == 1
                 select (p, (TAttribute)attr[0])).ToList());
        }

        internal static IEnumerable<(Type Type, TAttribute Attribute)> GetModelsWithAttribute<TAttribute>() 
            where TAttribute : Attribute
        {
            return (IEnumerable<(Type, TAttribute)>)_modelCache.GetOrAdd(typeof(TAttribute), _ =>
            {
                var results = new List<(Type, TAttribute)>();
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    try
                    {
                        foreach (var type in assembly.GetTypes())
                        {
                            if (type.IsDefined(typeof(TAttribute), true))
                            {
                                var attr = type.GetCustomAttribute(typeof(TAttribute), true) as TAttribute;
                                if (attr != null)
                                {
                                    results.Add((type, attr));
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Ignore assemblies that can't be loaded
                    }
                }
                return results;
            });
        }

        internal static IEnumerable<TAttribute> GetModelsWithAttribute<TAttribute>(object obj) 
            where TAttribute : Attribute
        {
            return obj.GetType()
                .GetCustomAttributes(typeof(TAttribute), true)
                .Cast<TAttribute>();
        }
  }
}
