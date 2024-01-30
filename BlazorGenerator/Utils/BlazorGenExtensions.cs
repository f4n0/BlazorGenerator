using BlazorGenerator.Models;
using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static BlazorGenerator.Utils.BlazorGenExtensions;

namespace BlazorGenerator.Utils
{
  public static class BlazorGenExtensions
  {
    public static void AddField<T>(this List<VisibleField<T>> visibleFields, string propertyName, AdditionalProperties<T> additionalProperties = null) where T : class
    {
      var prop = typeof(T).GetProperty(propertyName);


      var field = new VisibleField<T>()
      {
        Name = propertyName,
        fType = prop.PropertyType,
        Caption = propertyName,
        Getter = f => prop.GetValue(f),
        Setter = (f, v) => prop.SetValue(f, v)
      };

      if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
      {
        field.fType = Nullable.GetUnderlyingType(prop.PropertyType);
      }



      if (additionalProperties != null)
        additionalProperties(ref field);

      visibleFields.Add(field);
    }


    public delegate void AdditionalProperties<T>(ref VisibleField<T> reference) where T : class;

    public static Icon ToFluentIcon(this Type icon)
    {
      return Activator.CreateInstance(icon) as Icon;
    }
  }
}
