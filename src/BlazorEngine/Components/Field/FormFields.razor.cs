using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace BlazorEngine.Components.Field
{
  public partial class FormFields<T> : ComponentBase
  {
    private readonly string _id = Identifier.NewId();
    bool LookupOpen { get; set; } = false;
    private Dictionary<string, object> _commonAttributes = [];
    bool HasLookup { get; set; } = false;
    bool HasDrillDown { get; set; } = false;

    protected override Task OnParametersSetAsync()
    {
      var className = (Field.FieldType == typeof(bool)) ? "" : "FullSpanWidth";
      var styles = "";
      var color = Field.Color?.Invoke(Data);
      if (color != null)
        styles += "color: " + color.ToAttributeValue() + ";";

      _commonAttributes = new()
      {
        { "Id", _id },
        {"Appearance",FluentInputAppearance.Filled },
        {"ReadOnly", Field.ReadOnly || (Field.OnLookup != null) },
        {"style", styles },
        {"class", className },
        {"Immediate", Field.Immediate }
      };

      HasLookup = Field.OnLookup != null;
      HasDrillDown = Field.OnDrillDown != null;

      return base.OnParametersSetAsync();
    }

    void GenericOnClick()
    {
      if (HasLookup)
      {
        LookupOpen = true;
      }
    }
  }
}