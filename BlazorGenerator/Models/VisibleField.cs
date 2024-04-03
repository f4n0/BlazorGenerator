using BlazorGenerator.Enum;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator.Models
{
  public class VisibleField<T>
  {
    public string Caption { get; set; } = null!;
    public required Action<T, object> Setter { get; set; }
    public required Func<T, object?> Getter { get; set; }
    public required Type FieldType { get; set; }
    public required string Name { get; set; }
    public TextFieldType TextFieldType { get; set; } = TextFieldType.Text;
    public Func<T, string>? Href { get; set; }
    public Func<T, List<object>?>? OnLookup { get; set; }
    public Color? Color { get; set; }
    public bool Immediate { get; set; } = false;


    internal static VisibleField<T> NewField(string propertyName)
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

      return field;
    }
  }
}
