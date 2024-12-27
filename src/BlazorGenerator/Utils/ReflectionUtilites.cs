using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BlazorGenerator.Utils
{
  internal static class ReflectionUtilites
  {
    internal static void SetPropertyValueFromString(object target, PropertyInfo oProp, string propertyValue)
    {
      Type tProp = oProp.PropertyType;

      //Nullable properties have to be treated differently, since we
      //  use their underlying property to set the value in the object
      if (tProp.IsGenericType
          && tProp.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
      {
        //if it's null, just set the value from the reserved word null, and return
        if (propertyValue == null)
        {
          oProp.SetValue(target, null, null);
          return;
        }

        //Get the underlying type property instead of the nullable generic
        tProp = new NullableConverter(oProp.PropertyType).UnderlyingType;
      }

      //use the converter to get the correct value
      oProp.SetValue(target, Convert.ChangeType(propertyValue, tProp), null);
    }

    internal static async Task InvokeAction(MethodInfo Method, object target, object[]? KnownParams = null)
    {
      int? mthParams = Method.GetParameters().Length;
      var parameters = (mthParams.HasValue ? Enumerable.Repeat(Type.Missing, mthParams.Value).ToArray() : Array.Empty<object>());
      if (KnownParams != null && parameters.Length >= KnownParams.Length)
      {
        for (var i = 0; i < KnownParams.Length; i++)
        {
          parameters[i] = KnownParams[i];
        }
      }
      try
      {
        var ret = Method.Invoke(target, parameters);
        if (ret is Task task)
          await task.ConfigureAwait(true);
      }
      catch (TaskCanceledException)
      {
      }
    }


    internal static string GetCaption(PropertyInfo prop)
    {
      var attr = Attribute.GetCustomAttribute(prop, typeof(DisplayAttribute)) as DisplayAttribute;
      if (attr != null)
      {
        return attr.Name ?? prop.Name;
      }
      else
      {
        return prop.Name;
      }
    }

    internal static string? GetEnumCaption(MemberInfo? prop)
    {
      if (prop == null)
        return null;

      var attr = Attribute.GetCustomAttribute(prop, typeof(DisplayAttribute)) as DisplayAttribute;
      if (attr != null)
      {
        return attr.Name ?? prop.Name;
      }
      else
      {
        return prop.Name;
      }
    }
  }
}
