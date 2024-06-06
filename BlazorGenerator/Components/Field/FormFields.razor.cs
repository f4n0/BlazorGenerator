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

    protected override Task OnParametersSetAsync()
    {
      var styles = "width: 80%;";
      var color = Field.Color?.Invoke(Data);
      if (color != null)
        styles += "color: " + color.ToAttributeValue() + ";";

      commonAttributes = new()
      {
        { "Id", Id },
        {"Appearance",FluentInputAppearance.Filled },
        {"ReadOnly", !Editable || (Field.OnLookup != null) },
        {"style", styles },
        {"Immediate", Field.Immediate }
      };

      HasLookup = Field.OnLookup != null;

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