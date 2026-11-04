using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace BlazorEngine.Utils;

internal static class ReflectionUtilites
{
  private static readonly ConcurrentDictionary<Type, string[]> _enumNamesCache = new();
  private static readonly ConcurrentDictionary<Type, IReadOnlyDictionary<string, string>> _enumCaptionsCache = new();

  internal static string GetCaption(PropertyInfo prop)
  {
    var attr = Attribute.GetCustomAttribute(prop, typeof(DisplayAttribute)) as DisplayAttribute;
    if (attr != null) return attr.Name ?? prop.Name;

    return prop.Name;
  }

  internal static string? GetEnumCaption(MemberInfo? prop)
  {
    if (prop == null)
      return null;

    var attr = Attribute.GetCustomAttribute(prop, typeof(DisplayAttribute)) as DisplayAttribute;
    if (attr != null) return attr.Name ?? prop.Name;

    return prop.Name;
  }

  internal static Func<T, object?> GetPropertyGetter<T>(PropertyInfo prop)
  {
    if (prop.GetMethod is null)
      throw new ArgumentException($"Property '{prop.Name}' has no getter.", nameof(prop));

    // Value-type instances won't be updated correctly by Action<T,...> anyway;
    // and your models look class-based. Fail fast if someone tries structs.
    if (typeof(T).IsValueType || prop.DeclaringType?.IsValueType == true)
      throw new NotSupportedException("Typed property accessors are intended for reference types.");

    return PropertyAccessorCache<T>.Getters.GetOrAdd(prop, static p =>
    {
      var instanceParam = Expression.Parameter(typeof(T), "obj");

      // If prop is declared on a base type/interface, cast T to that declaring type.
      Expression instanceCast = p.DeclaringType == typeof(T)
        ? instanceParam
        : Expression.Convert(instanceParam, p.DeclaringType!);

      var propertyAccess = Expression.Property(instanceCast, p);
      var boxToObject = Expression.Convert(propertyAccess, typeof(object));

      return Expression.Lambda<Func<T, object?>>(boxToObject, instanceParam).Compile();
    });
  }

  internal static Action<T, object?> GetPropertySetter<T>(PropertyInfo prop)
  {
    if (prop.SetMethod is null)
      throw new ArgumentException($"Property '{prop.Name}' has no setter.", nameof(prop));

    if (typeof(T).IsValueType || prop.DeclaringType?.IsValueType == true)
      throw new NotSupportedException("Typed property accessors are intended for reference types.");

    return PropertyAccessorCache<T>.Setters.GetOrAdd(prop, static p =>
    {
      var instanceParam = Expression.Parameter(typeof(T), "obj");
      var valueParam = Expression.Parameter(typeof(object), "value");

      Expression instanceCast = p.DeclaringType == typeof(T)
        ? instanceParam
        : Expression.Convert(instanceParam, p.DeclaringType!);

      // Note: this will throw if value is null for non-nullable value types.
      var valueCast = Expression.Convert(valueParam, p.PropertyType);

      var assign = Expression.Assign(Expression.Property(instanceCast, p), valueCast);
      return Expression.Lambda<Action<T, object?>>(assign, instanceParam, valueParam).Compile();
    });
  }

  internal static string[] GetEnumNames(Type enumType)
  {
    return _enumNamesCache.GetOrAdd(enumType, static t => System.Enum.GetNames(t));
  }

  internal static string GetEnumCaption(Type enumType, string name)
  {
    var map = _enumCaptionsCache.GetOrAdd(enumType, static t =>
    {
      var dict = new Dictionary<string, string>(StringComparer.Ordinal);
      foreach (var n in System.Enum.GetNames(t))
      {
        var member = t.GetMember(n).FirstOrDefault();
        dict[n] = GetEnumCaption(member) ?? n;
      }

      return dict;
    });

    return map.TryGetValue(name, out var caption) ? caption : name;
  }

  private static class PropertyAccessorCache<T>
  {
    internal static readonly ConcurrentDictionary<PropertyInfo, Func<T, object?>> Getters = new();
    internal static readonly ConcurrentDictionary<PropertyInfo, Action<T, object?>> Setters = new();
  }
}
