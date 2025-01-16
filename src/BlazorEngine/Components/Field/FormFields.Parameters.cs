using BlazorEngine.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorEngine.Components.Field;

public partial class FormFields<T>
{
  [Parameter] public required VisibleField<T> Field { get; set; }

  [Parameter] public required T Data { get; set; }

  [Parameter] public bool ShowLabel { get; set; } = true;

  [Parameter] public bool IsTableCell { get; set; }

}