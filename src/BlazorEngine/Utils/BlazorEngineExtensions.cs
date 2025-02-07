using System.Diagnostics.CodeAnalysis;
using BlazorEngine.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine.Utils
{
  public static class BlazorEngineExtensions
  {
    public static void AddField<T>(this List<VisibleField<T>> visibleFields, string propertyName,
      AdditionalProperties<T>? additionalProperties) where T : class
    {
      var field = VisibleField<T>.NewField(propertyName);
      additionalProperties?.Invoke(ref field);
      visibleFields.Add(field);
    }
    
    public static VisibleField<T> AddCustomField<T>(this List<VisibleField<T>> visibleFields, string name,
       Func<T, VisibleField<T>, RenderFragment> customContent) where T : class
    {
      var field = new VisibleField<T>()
      {
        FieldType = typeof(object),
        Name = name,
        Caption = name,
        CustomContent = customContent
      };
      visibleFields.Add(field);
      return field;
    }

    public delegate void AdditionalProperties<T>(ref VisibleField<T> reference) where T : class;

    public static VisibleField<T> AddField<T>(this List<VisibleField<T>> visibleFields, string propertyName)
      where T : class
    {
      var field = VisibleField<T>.NewField(propertyName);

      visibleFields.Add(field);
      return field;
    }

    public static VisibleField<T> AddFieldProperty<T>(this VisibleField<T> field,
      Action<VisibleField<T>>? additionalProperty) where T : class
    {
      additionalProperty?.Invoke(field);
      return field;
    }

    [RequiresUnreferencedCode("DynamicBehavior is incompatible with trimming.")]
    public static Icon? ToFluentIcon(this Type icon)
    {
      return Activator.CreateInstance(icon) as Icon;
    }

    public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
    {
      if (list == null) throw new ArgumentNullException(nameof(list));
      if (items == null) throw new ArgumentNullException(nameof(items));

      if (list is List<T> asList)
      {
        asList.AddRange(items);
      }
      else
      {
        foreach (var item in items)
        {
          list.Add(item);
        }
      }
    }
    
    public static VisibleField<T> Configure<T>(this VisibleField<T> field, Action<FieldBuilder<T>> config)
    {
      var builder = new FieldBuilder<T>(field);
      config(builder);
      return builder.Build();
    }
  }
}