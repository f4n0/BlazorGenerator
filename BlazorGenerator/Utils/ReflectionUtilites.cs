using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

    internal static T CopyObject<T>(this T obj) where T : new()
    {
      var type = obj.GetType();
      var props = type.GetProperties();
      var fields = type.GetFields();
      var copyObj = new T();
      foreach (var item in props)
      {
        item.SetValue(copyObj, item.GetValue(obj));
      }
      foreach (var item in fields)
      {
        item.SetValue(copyObj, item.GetValue(obj));
      }
      return copyObj;
    }
  }
}
