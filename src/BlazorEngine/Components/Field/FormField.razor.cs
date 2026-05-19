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

  private Dictionary<Type, RenderFragment>? _typeSwitch;
  private object? _currentValue;
  private bool _isValueLoading;
  private int _currentValueVersion;

  private bool LookupOpen;

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

    if (Field.Get != null)
    {
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


