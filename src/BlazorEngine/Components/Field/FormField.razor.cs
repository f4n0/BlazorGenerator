using BlazorEngine.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace BlazorEngine.Components.Field;

public partial class FormField<T>
{
  private static readonly Icon DrillDownIcon = new Size16.MoreHorizontal();
  private static readonly Icon LookupIcon = new Size16.ChevronDown();
  private readonly Dictionary<string, object> _commonAttributes = [];
  private readonly string _id = Identifier.NewId();

  private Dictionary<Type, RenderFragment>? _typeSwitch;
  private object? _currentValue;
  private bool _isValueLoading;
  private int _currentValueVersion;
  private VisibleField<T>? _currentValueField;
  private T? _currentValueData;

  private FluentMenu? _lookupMenu;
  private bool _lookupOpen;

  [Parameter] public required VisibleField<T> Field { get; set; }
  [Parameter] public required T Data { get; set; }

  [Parameter] public bool ShowLabel { get; set; } = true;

  [Parameter] public bool IsTableCell { get; set; }

  private bool EffectiveShowLabel => ShowLabel && Field.FieldType != typeof(Action);

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

  private object? CurrentValue => Field.Get == null ? Field.InternalGet(Data) : _currentValue;

  private void SetFieldValue(object? value)
  {
    Field.InternalSet(Data, value);

    if (Field.Get != null)
    {
      // A custom setter may transform a lookup key before storing it in the model.
      var version = ++_currentValueVersion;
      var currentValue = Field.InternalGetAsync(Data);
      if (currentValue.IsCompletedSuccessfully)
      {
        _currentValue = currentValue.Result;
        _isValueLoading = false;
      }
      else
      {
        _isValueLoading = true;
        _ = LoadCurrentValueAsync(version, currentValue);
      }
    }
  }

  private Task GenericOnClick()
  {
    return Field.OnLookup != null && _lookupMenu != null && !_lookupOpen
      ? _lookupMenu.OpenMenuAsync(_id)
      : Task.CompletedTask;
  }

  private RenderFragment NumberField<TNumber>(Func<object?, TNumber> convert) => builder =>
  {
    builder.OpenComponent<FluentNumberInput<TNumber>>(0);
    builder.AddMultipleAttributes(1, _commonAttributes);
    builder.AddAttribute(2, nameof(FluentNumberInput<TNumber>.Value), convert(CurrentValue));
    builder.AddAttribute(3, nameof(FluentNumberInput<TNumber>.ValueChanged),
      EventCallback.Factory.Create<TNumber>(this, value => SetFieldValue(value)));
    builder.AddAttribute(4, nameof(FluentNumberInput<TNumber>.Appearance), TextInputAppearance.FilledDarker);
    builder.AddAttribute(5, nameof(FluentNumberInput<TNumber>.Placeholder), Field.PlaceHolder);
    builder.AddAttribute(6, nameof(FluentNumberInput<TNumber>.Immediate), Field.Immediate);
    builder.AddAttribute(7, nameof(FluentNumberInput<TNumber>.EndTemplate), DrillDownOrLookup());
    builder.CloseComponent();
  };

  protected override Task OnParametersSetAsync()
  {
    var className = "form-field-control";
    var styles = "width: 100%; min-width: 0; max-width: 100%; margin: 0;";
    var color = Field.Color?.Invoke(Data);
    if (color != null)
      styles += "color: " + color.ToAttributeValue() + ";";
    styles += Field.CssStyle;
    className += $" {Field.CssClass}";

    _commonAttributes.Clear();
    _commonAttributes["Id"] = _id;
    _commonAttributes["Label"] = EffectiveShowLabel ? Field.Caption : string.Empty;
    _commonAttributes["LabelPosition"] = LabelPosition.Before;
    _commonAttributes["LabelWidth"] = "20%";
    _commonAttributes["Required"] = Field.Required;
    _commonAttributes["AriaLabel"] = Field.Caption;
    _commonAttributes["Tooltip"] = Field.Tooltip ?? Field.Caption;
    _commonAttributes["ReadOnly"] = Field.ReadOnly || Field.OnLookup != null;
    _commonAttributes["style"] = styles;
    _commonAttributes["class"] = className;

    if (Field.Get != null)
    {
      if (EqualityComparer<VisibleField<T>>.Default.Equals(_currentValueField, Field) &&
          EqualityComparer<T>.Default.Equals(_currentValueData, Data))
        return base.OnParametersSetAsync();

      _currentValueField = Field;
      _currentValueData = Data;
      _currentValue = null;
      var value = Field.Get(new VisibleFieldGetterArgs<T>
      {
        Field = Field,
        Data = Data
      });

      switch (value)
      {
        case ValueTask<object?> vt:
          if (vt.IsCompletedSuccessfully)
          {
            _currentValue = vt.Result;
            _isValueLoading = false;
          }
          else
          {
            _isValueLoading = true;
            _currentValueVersion++;
            _ = LoadCurrentValueAsync(_currentValueVersion, vt);
          }
          break;
        case Task<object?> task:
          if (task.IsCompletedSuccessfully)
          {
            _currentValue = task.Result;
            _isValueLoading = false;
          }
          else
          {
            _isValueLoading = true;
            _currentValueVersion++;
            _ = LoadCurrentValueAsync(_currentValueVersion, new ValueTask<object?>(task));
          }
          break;
        default:
          _currentValue = value;
          _isValueLoading = false;
          break;
      }
    }
    else
    {
      _currentValueField = Field;
      _currentValueData = Data;
      _isValueLoading = false;
    }

    return base.OnParametersSetAsync();
  }

  private async Task LoadCurrentValueAsync(int version, ValueTask<object?> valueTask)
  {
    try
    {
      _currentValue = await valueTask;
    }
    catch
    {
      if (version != _currentValueVersion)
        return;

      _currentValue = null;
    }
    finally
    {
      if (version == _currentValueVersion)
      {
        _isValueLoading = false;
        await InvokeAsync(StateHasChanged);
      }
    }
  }
}


