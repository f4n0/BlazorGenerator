using BlazorGenerator.Enum;
using BlazorGenerator.Utils;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator.Models
{
  public class VisibleField<T>
  {
    public string Caption { get; set; } = null!;

    [Obsolete("This will be removed in next major, use Set instead")]
    public Action<T, object>? Setter { get; set; }
    [Obsolete("This will be removed in next major, use Get instead")]
    public Func<T, object?>? Getter { get; set; }

    public Action<VisibleFieldSetterArgs<T>>? Set { get; set; }
    public Func<VisibleFieldGetterArgs<T>, object?>? Get { get; set; }


    public required Type FieldType { get; set; }
    public required string Name { get; set; }
    public TextFieldType TextFieldType { get; set; } = TextFieldType.Text;
    public Func<T, string>? Href { get; set; }
    public Func<T, Dictionary<object, string>?>? OnLookup { get; set; }
    public bool Immediate { get; set; } = false;
    public bool ReadOnly { get; set; } = false;
    public Func<T, TextStyle>? TextStyle { get; set; }
    public Func<T, Color>? Color { get; set; }
    public Action<VisibleFieldDrillDownArgs<T>>? OnDrillDown { get; set; }
    public string? Tooltip { get; set; }
    public bool Required { get; set; }
    public string? PlaceHolder { get; set; } = string.Empty;
    public string? Group { get; set; } = string.Empty;

    internal static VisibleField<T> NewField(string propertyName)
    {
      var prop = typeof(T).GetProperty(propertyName) ?? throw new Exception("Cannot find property with name \"" + propertyName + "\"");

      var field = new VisibleField<T>()
      {
        Name = propertyName,
        FieldType = prop.PropertyType,
        Caption = ReflectionUtilites.GetCaption(prop),
        Get = (args) => prop.GetValue(args.Data),
        Set = (args) => prop.SetValue(args.Data, args.Value)
      };

      if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
      {
#pragma warning disable CS8601 // Possible null reference assignment.
        field.FieldType = Nullable.GetUnderlyingType(prop.PropertyType);
#pragma warning restore CS8601 // Possible null reference assignment.
      }

      return field;
    }

    internal object? InternalGet(T data)
    {
      if (Get != null)
      {
        return Get.Invoke(new VisibleFieldGetterArgs<T>()
        {
          Field = this,
          Data = data
        });
#pragma warning disable CS0618 // Type or member is obsolete
      }
      else
      {
        return Getter?.Invoke(data);
      }
#pragma warning restore CS0618 // Type or member is obsolete
    }

    internal void InternalSet(T data, object? value)
    {
      if (Set != null)
      {
        Set.Invoke(new VisibleFieldSetterArgs<T>()
        {
          Field = this,
          Data = data,
          Value = value
        });
      }
      else
      {
#pragma warning disable CS0618 // Type or member is obsolete
        Setter?.Invoke(data, value!);
#pragma warning restore CS0618 // Type or member is obsolete
      }

    }

    internal void InternalDrillDown(T Data)
    {
      if (OnDrillDown != null)
      {
        OnDrillDown.Invoke(new VisibleFieldDrillDownArgs<T>()
        {
          Data = Data,
          Field = this
        });
      }
    }
  }
}
