using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace BlazorGenerator.Components.Field
{
  public partial class FormFields<T> : ComponentBase
  {
    private readonly string Id = Identifier.NewId();
    bool LookupOpen { get; set; } = false;
    private Dictionary<string, object> commonAttributes = [];
    bool HasLookup { get; set; } = false;
    bool HasDrillDown { get; set; } = false;

    protected override Task OnParametersSetAsync()
    {
      var styles = "min-width: 75%;";
      var color = Field.Color?.Invoke(Data);
      if (color != null)
        styles += "color: " + color.ToAttributeValue() + ";";

      commonAttributes = new()
      {
        { "Id", Id },
        {"Appearance",FluentInputAppearance.Filled },
        {"ReadOnly", Field.ReadOnly || (Field.OnLookup != null) },
        {"style", styles },
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