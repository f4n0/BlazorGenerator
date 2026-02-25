using BlazorEngine.Enum;
using BlazorEngine.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine.Utils;

public class FieldBuilder<T>(VisibleField<T> field)
{
  public FieldBuilder<T> FieldType(Type type)
  {
    field.FieldType = type;
    return this;
  }

  public FieldBuilder<T> Name(string name)
  {
    field.Name = name;
    return this;
  }

  public FieldBuilder<T> Caption(string caption)
  {
    field.Caption = caption;
    return this;
  }

  public FieldBuilder<T> Tooltip(string? tooltip)
  {
    field.Tooltip = tooltip;
    return this;
  }

  public FieldBuilder<T> Placeholder(string? placeholder)
  {
    field.PlaceHolder = placeholder;
    return this;
  }

  public FieldBuilder<T> Group(string? group)
  {
    field.Group = group;
    return this;
  }

  public FieldBuilder<T> Multiline(bool multiline = true)
  {
    field.Multiline = multiline;
    return this;
  }

  public FieldBuilder<T> Set(Action<VisibleFieldSetterArgs<T>>? setAction)
  {
    field.Set = setAction;
    return this;
  }

  public FieldBuilder<T> OnChange(Action<VisibleFieldSetterArgs<T>>? onChange)
  {
    field.OnChange = onChange;
    return this;
  }

  public FieldBuilder<T> Get(Func<VisibleFieldGetterArgs<T>, object?>? getFunc)
  {
    field.Get = getFunc;
    return this;
  }

  public FieldBuilder<T> Immediate(bool immediate = true)
  {
    field.Immediate = immediate;
    return this;
  }

  public FieldBuilder<T> Additional(bool additional = true)
  {
    field.Additional = additional;
    return this;
  }

  public FieldBuilder<T> TextFieldType(TextFieldType type)
  {
    field.TextFieldType = type;
    return this;
  }

  public FieldBuilder<T> ReadOnly(bool readOnly = true)
  {
    field.ReadOnly = readOnly;
    return this;
  }

  public FieldBuilder<T> TextStyle(Func<T, TextStyle>? textStyleFunc)
  {
    field.TextStyle = textStyleFunc;
    return this;
  }

  public FieldBuilder<T> Color(Func<T, Color>? colorFunc)
  {
    field.Color = colorFunc;
    return this;
  }

  public FieldBuilder<T> Required(bool required = true)
  {
    field.Required = required;
    return this;
  }

  public FieldBuilder<T> EnableSearch(bool enableSearch = true)
  {
    field.EnableSearch = enableSearch;
    return this;
  }

  public FieldBuilder<T> Href(Func<T, string>? hrefFunc)
  {
    field.Href = hrefFunc;
    return this;
  }

  public FieldBuilder<T> OnLookup(Func<T, Dictionary<object, string>?>? lookupFunc)
  {
    field.OnLookup = lookupFunc;
    return this;
  }

  public FieldBuilder<T> OnDrillDown(Action<VisibleFieldDrillDownArgs<T>>? drillDownAction)
  {
    field.OnDrillDown = drillDownAction;
    return this;
  }

  public FieldBuilder<T> CustomContent(Func<T, VisibleField<T>, RenderFragment>? contentFunc)
  {
    field.CustomContent = contentFunc;
    return this;
  }

  internal VisibleField<T> Build()
  {
    return field;
  }
}