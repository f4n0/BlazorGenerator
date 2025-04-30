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
      
      _commonAttributes = new()
      {
        { "Id", _id },
        {"Appearance", Field.FieldType == typeof(Action) ? Appearance.Accent : FluentInputAppearance.Filled },
        {"ReadOnly", Field.ReadOnly || (Field.OnLookup != null) },
        {"style", styles },
        {"class", className },
        {"Immediate", Field.Immediate }
      };

      if (Field.FieldType == typeof(Action))
        ShowLabel = false;

      return base.OnParametersSetAsync();
    }
  }
}