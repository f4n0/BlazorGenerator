using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace BlazorEngine.Utils;

internal static class ReflectionUtilites
{
  private static readonly ConcurrentDictionary<PropertyInfo, Action<object, object?>> _propertySetters = new();
  private static readonly ConcurrentDictionary<MethodInfo, Func<object, object[], object?>> _methodInvokers = new();

  private static readonly ConcurrentDictionary<Type, string[]> _enumNamesCache = new();
  private static readonly ConcurrentDictionary<Type, IReadOnlyDictionary<string, string>> _enumCaptionsCache = new();

  internal static void SetPropertyValueFromString(object target, PropertyInfo oProp, string? propertyValue)
  {
    var tProp = oProp.PropertyType;

    //Nullable properties have to be treated differently, since we
    //  use their underlying property to set the value in the object
    if (tProp.IsGenericType
        && tProp.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
    {
      //if it's null, just set the value from the reserved word null, and return
      if (propertyValue == null)
      {
        GetPropertySetter(oProp)(target, null);
        return;
      }

      //Get the underlying type property instead of the nullable generic
      tProp = Nullable.GetUnderlyingType(oProp.PropertyType)!;
    }

    //use the converter to get the correct value
    var val = Convert.ChangeType(propertyValue, tProp);
    GetPropertySetter(oProp)(target, val);
  }

  private static Action<object, object?> GetPropertySetter(PropertyInfo prop)
  {
    return _propertySetters.GetOrAdd(prop, p =>
    {
      var targetParam = Expression.Parameter(typeof(object), "target");
      var valueParam = Expression.Parameter(typeof(object), "value");

      Expression castTarget;
      if (p.DeclaringType!.IsValueType)
        // Unbox allows in-place modification if target is a boxed struct
        castTarget = Expression.Unbox(targetParam, p.DeclaringType);
      else
        castTarget = Expression.Convert(targetParam, p.DeclaringType);

      var castValue = Expression.Convert(valueParam, p.PropertyType);
      var assign = Expression.Assign(Expression.Property(castTarget, p), castValue);

      return Expression.Lambda<Action<object, object?>>(assign, targetParam, valueParam).Compile();
    });
  }

  internal static async Task InvokeAction(MethodInfo method, object target, object[]? knownParams = null)
  {
    var mthParams = method.GetParameters().Length;
    object[] parameters;
    if (mthParams == 0)
    {
      parameters = Array.Empty<object>();
    }
    else
    {
      parameters = new object[mthParams];
      Array.Fill(parameters, Type.Missing);
    }

    if (knownParams != null && parameters.Length >= knownParams.Length)
      for (var i = 0; i < knownParams.Length; i++)
        parameters[i] = knownParams[i];

    try
    {
      var ret = GetMethodInvoker(method)(target, parameters);
      if (ret is Task task)
        await task.ConfigureAwait(false);
    }
    catch (TaskCanceledException)
    {
    }
  }

  private static Func<object, object[], object?> GetMethodInvoker(MethodInfo method)
  {
    return _methodInvokers.GetOrAdd(method, m =>
    {
      var targetParam = Expression.Parameter(typeof(object), "target");
      var argsParam = Expression.Parameter(typeof(object[]), "args");

      Expression castTarget;
      if (m.DeclaringType!.IsValueType)
        castTarget = Expression.Unbox(targetParam, m.DeclaringType);
      else
        castTarget = Expression.Convert(targetParam, m.DeclaringType);

      var parameters = m.GetParameters();
      var callArgs = new Expression[parameters.Length];

      for (var i = 0; i < parameters.Length; i++)
      {
        var paramInfo = parameters[i];
        var index = Expression.Constant(i);
        var accessor = Expression.ArrayIndex(argsParam, index);

        // Handle Type.Missing by checking reference equality and swapping with default value
        var isMissing = Expression.ReferenceEqual(accessor, Expression.Constant(Type.Missing));

        Expression defaultValue;
        if (paramInfo.HasDefaultValue)
          defaultValue = Expression.Constant(paramInfo.DefaultValue, paramInfo.ParameterType);
        else
          defaultValue = Expression.Default(paramInfo.ParameterType);

        var castValue = Expression.Convert(accessor, paramInfo.ParameterType);
        callArgs[i] = Expression.Condition(isMissing, defaultValue, castValue);
      }

      var call = Expression.Call(castTarget, m, callArgs);

      Expression body;
      if (m.ReturnType == typeof(void))
        body = Expression.Block(call, Expression.Constant(null));
      else
        body = Expression.Convert(call, typeof(object));

      return Expression.Lambda<Func<object, object[], object?>>(body, targetParam, argsParam).Compile();
    });
  }

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