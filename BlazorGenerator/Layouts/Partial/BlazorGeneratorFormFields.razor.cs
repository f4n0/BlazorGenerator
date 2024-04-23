using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace BlazorGenerator.Layouts.Partial
{
  public partial class BlazorGeneratorFormFields<T> : ComponentBase
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

    private TypeCode GetFieldType()
    {
      return Type.GetTypeCode(Field.FieldType);
    }

    protected override Task OnParametersSetAsync()
    {
      var styles = "width: 80%;";
      if (Field.Color != null)
        styles += "color: " + Field.Color.ToAttributeValue() + ";";
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
