using System.Linq.Expressions;
using System.Reflection;
using BlazorEngine.Models;
using BlazorEngine.Utils;
using Microsoft.AspNetCore.Components;

namespace BlazorEngine.Fields;

public sealed class FieldListBuilder<T>
{
  private readonly List<VisibleField<T>> _fields = [];

  public FieldBuilder<T> Add<TValue>(Expression<Func<T, TValue>> expression)
  {
    // Bind the actual member rather than looking its name up a second time.
    if (expression.Body is not MemberExpression { Member: PropertyInfo property, Expression: var target } ||
        target != expression.Parameters[0] || property.GetIndexParameters().Length != 0)
      throw new ArgumentException("Expression must select a property directly on the model.", nameof(expression));

    var field = VisibleField<T>.NewField(property);
    _fields.Add(field);
    return new FieldBuilder<T>(field);
  }

  public FieldBuilder<T> Add(string propertyName)
  {
    var field = VisibleField<T>.NewField(propertyName);
    _fields.Add(field);
    return new FieldBuilder<T>(field);
  }

  public FieldBuilder<T> Custom(string name, Func<T, VisibleField<T>, RenderFragment> content)
  {
    var field = new VisibleField<T>
    {
      Name = name,
      Caption = name,
      FieldType = typeof(object),
      CustomContent = content
    };
    _fields.Add(field);
    return new FieldBuilder<T>(field);
  }

  public FieldListBuilder<T> Add(VisibleField<T> field)
  {
    _fields.Add(field);
    return this;
  }

  internal List<VisibleField<T>> Build() => _fields;
}
