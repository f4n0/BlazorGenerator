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
    public static void AddField<T>(this List<VisibleField<T>> visibleFields, string propertyName, AdditionalProperties<T>? additionalProperties = null) where T : class
    {
      var prop = typeof(T).GetProperty(propertyName) ?? throw new NullReferenceException("Cannot find property with name \"" + propertyName + "\"");

      var field = new VisibleField<T>()
      {
        Name = propertyName,
        FieldType = prop.PropertyType,
        Caption = propertyName,
        Getter = f => prop.GetValue(f),
        Setter = (f, v) => prop.SetValue(f, v)
      };

      if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
      {
#pragma warning disable CS8601 // Possible null reference assignment.
        field.FieldType = Nullable.GetUnderlyingType(prop.PropertyType);
#pragma warning restore CS8601 // Possible null reference assignment.
      }

      additionalProperties?.Invoke(ref field);

      visibleFields.Add(field);
    }

    public delegate void AdditionalProperties<T>(ref VisibleField<T> reference) where T : class;

    public static Icon? ToFluentIcon(this Type icon)
    {
      return Activator.CreateInstance(icon) as Icon;
    }
  }
}
