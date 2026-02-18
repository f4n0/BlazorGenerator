using BlazorEngine.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace BlazorEngine.Components.Field
{
  public partial class FormField<T>
  {
    [Parameter] public required VisibleField<T> Field { get; set; }
    [Parameter] public required T Data { get; set; }

    [Parameter] public bool ShowLabel { get; set; } = true;

    [Parameter] public bool IsTableCell { get; set; }

    private bool LookupOpen = false;
    private Dictionary<string, object> _commonAttributes = [];
    private readonly string _id = Identifier.NewId();

    void GenericOnClick()
    {
      if (Field.OnLookup != null)
      {
        LookupOpen = true;
      }
    }

    protected override Task OnParametersSetAsync()
    {
      var className = (Field.FieldType == typeof(bool) || Field.FieldType == typeof(Action)) ? "" : "FullSpanWidth";
      var styles = "";
      var color = Field.Color?.Invoke(Data);
      if (color != null)
        styles += "color: " + color.ToAttributeValue() + ";";
      styles += Field.CssStyle;
      className += $" {Field.CssClass}";

      _commonAttributes.Clear();
      _commonAttributes["Id"] = _id;
      _commonAttributes["Appearance"] = (Field.FieldType == typeof(Action))
        ? Appearance.Accent
        : FluentInputAppearance.Filled;
      if ((Field.FieldType?.IsEnum ?? false))
      {
        _commonAttributes["Appearance"] = Appearance.Filled;
      }

      _commonAttributes["ReadOnly"] = Field.ReadOnly || (Field.OnLookup != null);
      _commonAttributes["style"] = styles;
      _commonAttributes["class"] = className;
      _commonAttributes["Immediate"] = Field.Immediate;

      if (Field.FieldType == typeof(Action))
        ShowLabel = false;

      return base.OnParametersSetAsync();
    }

    private Dictionary<Type, RenderFragment>? _typeSwitch;

    private Dictionary<Type, RenderFragment> TypeSwitch => _typeSwitch ??= new()
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

    protected override bool ShouldRender()
    {
      // Only re-render if Data or Field reference changes
      // (You can add more sophisticated checks if needed)
      return _lastData?.GetHashCode() != Data?.GetHashCode() || _lastField != Field;
    }

    private T? _lastData;
    private VisibleField<T>? _lastField;

    protected override void OnAfterRender(bool firstRender)
    {
      _lastData = Data;
      _lastField = Field;
    }
  }
}