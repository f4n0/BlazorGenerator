using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator.Layouts.Partial
{
  public partial class BlazorGeneratorFormFields<T> : ComponentBase
  {
    [Parameter]
    public required VisibleField<T> field { get; set; }
    [Parameter]
    public required T Data { get; set; }
    [Parameter]
    public bool showLabel { get; set; } = true;
    [Parameter]
    public bool Editable { get; set; } = true;
    [Parameter]
    public bool isTableCell { get; set; } = false;
    [Parameter]
    public bool hasLookup { get; set; } = false;

    private string Id = Identifier.NewId();
    bool LookupOpen { get; set; } = false;

    private Dictionary<string, object> commonAttributes = [];

    private TypeCode GetFieldType()
    {

      var temp = Type.GetTypeCode(field.FieldType);
      var temp1 = Type.GetTypeCode(field.FieldType.BaseType);
      return temp;
    }

    protected override Task OnParametersSetAsync()
    {
      commonAttributes = new()
      {
        { "Id", Id },
        {"Appearance",FluentInputAppearance.Filled },
        {"ReadOnly", !Editable },
        {"role", "password" }
      };

      hasLookup = field.OnLookup != null;

      return base.OnParametersSetAsync();
    }
  }
}
