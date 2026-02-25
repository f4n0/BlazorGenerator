using BlazorEngine.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;

namespace BlazorEngine.Components.Field;

public partial class FormField<T>
{
  private static readonly Icon DrillDownIcon = new Size16.MoreHorizontal();
  private static readonly Icon LookupIcon = new Size16.ChevronDown();
  private readonly Dictionary<string, object> _commonAttributes = [];
  private readonly string _id = Identifier.NewId();

  private T? _lastData;
  private VisibleField<T>? _lastField;

  private Dictionary<Type, RenderFragment>? _typeSwitch;

  private bool LookupOpen;

  [Parameter] public required VisibleField<T> Field { get; set; }
  [Parameter] public required T Data { get; set; }

  [Parameter] public bool ShowLabel { get; set; } = true;

  [Parameter] public bool IsTableCell { get; set; }

  private Dictionary<Type, RenderFragment> TypeSwitch => _typeSwitch ??= new Dictionary<Type, RenderFragment>
  {
    { typeof(bool), BoolField },
    { typeof(short), ShortField },
    { typeof(ushort), UShortField },
    { typeof(int), IntField },
    { typeof(uint), UIntField },
    { typeof(long), LongField },
    { typeof(ulong), UlongField },
    { typeof(float), FloatField },
    { typeof(double), DoubleField },
    { typeof(decimal), DecimalField },
    { typeof(string), TextField },
    { typeof(DateTime), DateTimeField },
    { typeof(Type), IconField },
    { typeof(Action), ActionField }
  };

  private void GenericOnClick()
  {
    if (Field.OnLookup != null) LookupOpen = true;
  }

  protected override Task OnParametersSetAsync()
  {
    var className = Field.FieldType == typeof(bool) || Field.FieldType == typeof(Action) ? "" : "FullSpanWidth";
    var styles = "";
    var color = Field.Color?.Invoke(Data);
    if (color != null)
      styles += "color: " + color.ToAttributeValue() + ";";
    styles += Field.CssStyle;
    className += $" {Field.CssClass}";

    _commonAttributes.Clear();
    _commonAttributes["Id"] = _id;
    _commonAttributes["Appearance"] = Field.FieldType == typeof(Action)
      ? Appearance.Accent
      : FluentInputAppearance.Filled;
    if (Field.FieldType?.IsEnum ?? false) _commonAttributes["Appearance"] = Appearance.Filled;

    _commonAttributes["ReadOnly"] = Field.ReadOnly || Field.OnLookup != null;
    _commonAttributes["style"] = styles;
    _commonAttributes["class"] = className;
    _commonAttributes["Immediate"] = Field.Immediate;

    if (Field.FieldType == typeof(Action))
      ShowLabel = false;

    return base.OnParametersSetAsync();
  }

  protected override bool ShouldRender()
  {
    return !ReferenceEquals(_lastData, Data) || !ReferenceEquals(_lastField, Field);
  }

  protected override void OnAfterRender(bool firstRender)
  {
    _lastData = Data;
    _lastField = Field;
  }
}