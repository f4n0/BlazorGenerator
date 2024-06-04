using System.Reflection;
using BlazorGenerator.Attributes;
using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorGenerator.Components.DataGrid;

public partial class ListDataGrid<T>
{
  [Parameter] public required object Context { get; set; }

  [Parameter] public required List<VisibleField<T>> VisibleFields { get; set; }

  [Parameter] public IQueryable<T>? Data { get; set; }

  [Parameter] public bool ShowButtons { get; set; }

  [Parameter] public PermissionSet? PermissionSet { get; set; }

  [Parameter] public IEnumerable<(MethodInfo Method, GridActionAttribute Attribute)>? GridActions { get; set; } = [];

  [Parameter] public List<T> Selected { get; set; } = [];

  [Parameter] public EventCallback<List<T>> SelectedChanged { get; set; }

  [Parameter] public Action? RefreshData { get; set; }

  [Parameter] public virtual Type? EditFormType { get; set; }

  [Parameter] public Action<T>? OnSave { get; set; }

  [Parameter] public Action<T>? OnDiscard { get; set; }

  [Parameter] public Func<T>? OnNewItem { get; set; }
}