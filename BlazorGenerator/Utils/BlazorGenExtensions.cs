using BlazorGenerator.Models;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace BlazorGenerator.Utils
{
  public static class BlazorGenExtensions
  {
    public static void AddField<T>(this List<VisibleField<T>> visibleFields, string propertyName, AdditionalProperties<T>? additionalProperties = null) where T : class
    {
      var field = VisibleField<T>.NewField(propertyName);
      additionalProperties?.Invoke(ref field);
      visibleFields.Add(field);
    }
    public delegate void AdditionalProperties<T>(ref VisibleField<T> reference) where T : class;

    public static VisibleField<T> AddField<T>(this List<VisibleField<T>> visibleFields, string propertyName) where T : class
    {
      var field = VisibleField<T>.NewField(propertyName);

      visibleFields.Add(field);
      return field;
    }
    public static VisibleField<T> AddFieldProperty<T>(this VisibleField<T> field, Action<VisibleField<T>>? AdditionalProperty) where T : class
    {
      AdditionalProperty?.Invoke(field);
      return field;
    }

    [RequiresUnreferencedCode("DynamicBehavior is incompatible with trimming.")]
    public static Icon? ToFluentIcon(this Type icon)
    {
      return Activator.CreateInstance(icon) as Icon;
    }
  }
}
