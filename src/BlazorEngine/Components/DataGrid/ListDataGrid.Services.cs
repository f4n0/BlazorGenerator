using BlazorEngine.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorEngine.Components.DataGrid;

public partial class ListDataGrid<T>
{
  [Inject] public UIServices? UIServices { get; set; }

  [Inject] private IJSRuntime? JSRuntime { get; set; }
}