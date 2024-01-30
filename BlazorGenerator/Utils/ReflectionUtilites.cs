using BlazorGenerator.Layouts;
using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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

  }
}
