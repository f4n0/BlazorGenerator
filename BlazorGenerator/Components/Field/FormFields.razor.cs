using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace BlazorGenerator.Components.Field
{
  public partial class FormFields<T> : ComponentBase
  {
    [Parameter]
    public required VisibleField<T> Field { get; set; }
    [Parameter]
    public required T Data { get; set; }
    [Parameter]
    public bool ShowLabel { get; set; } = true;
    [Parameter]
    public bool Editable { get; set; } = true;
    [Parameter]
    public bool IsTableCell { get; set; } = false;
    [Parameter]
    public bool HasLookup { get; set; } = false;

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
        {"ReadOnly", !Editable },
        {"role", "password" },
        {"style", styles },
        {"Immediate", Field.Immediate }
      };

      HasLookup = Field.OnLookup != null;

      return base.OnParametersSetAsync();
    }
  }
}
