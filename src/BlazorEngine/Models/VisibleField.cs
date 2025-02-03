using BlazorEngine.Enum;
using BlazorEngine.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine.Models
{
  public class VisibleField<T>
  {
    /// <summary>
    /// The type of the original field. Used for correct cast
    /// </summary>
    public required Type FieldType { get; set; }

    /// <summary>
    /// The unique name of the field
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The caption, this will be the name visible to the users
    /// </summary>
    public string Caption { get; set; } = null!;

    /// <summary>
    /// The tooltip of the field, shown when the user hoover on the field
    /// </summary>
    public string? Tooltip { get; set; }

    /// <summary>
    /// The placeholder text for the field
    /// </summary>
    public string? PlaceHolder { get; set; } = string.Empty;

    /// <summary>
    /// The fields can be grouped based on this field, default is Empty (no group created)
    /// </summary>
    public string? Group { get; set; } = string.Empty;
    public bool Multiline { get; set; }
    public Action<VisibleFieldSetterArgs<T>>? Set { get; set; }
    public Action<VisibleFieldSetterArgs<T>>? OnChange { get; set; }
    public Func<VisibleFieldGetterArgs<T>, object?>? Get { get; set; }
    public bool Immediate { get; set; } = false;
    public bool Additional { get; set; } = false;
    public TextFieldType TextFieldType { get; set; } = TextFieldType.Text;
    public bool ReadOnly { get; set; } = false;
    public Func<T, TextStyle>? TextStyle { get; set; }
    public Func<T, Color>? Color { get; set; }
    public bool Required { get; set; }
    public bool EnableSearch { get; set; } = false;
    public Func<T, string>? Href { get; set; }
    public Func<T, Dictionary<object, string>?>? OnLookup { get; set; }
    public Action<VisibleFieldDrillDownArgs<T>>? OnDrillDown { get; set; }
    
    public Func<T, VisibleField<T>,RenderFragment>? CustomContent { get; set; }


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

    internal object? InternalGet(T data) =>
      Get?.Invoke(new VisibleFieldGetterArgs<T>()
      {
        Field = this,
        Data = data
      });

    internal void InternalChange(T data, string? value) =>
    OnChange?.Invoke(new VisibleFieldSetterArgs<T>()
      {
        Field = this,
        Data = data,
        Value = value
      });

    internal void InternalSet(T data, object? value) =>
      Set?.Invoke(new VisibleFieldSetterArgs<T>()
      {
        Field = this,
        Data = data,
        Value = value
      });

    internal void InternalDrillDown(T data) =>
      OnDrillDown?.Invoke(new VisibleFieldDrillDownArgs<T>()
      {
        Data = data,
        Field = this
      });
  }

}
