﻿using System.Collections.ObjectModel;
using System.Reflection;
using BlazorEngine.Attributes;
using BlazorEngine.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorEngine.Components.DataGrid;

public partial class ListDataGrid<T>
{

  [Parameter] public required object Context { get; set; }

  [Parameter] public required List<VisibleField<T>> VisibleFields { get; set; }

  private IQueryable<T>? _data;

  [Parameter] 
  public IQueryable<T>? Data { get => _data; set { 
      _data = value;
      Selected.Clear();
    } }

  [Parameter] public bool ShowButtons { get; set; }

  [Parameter] public PermissionSet? PermissionSet { get; set; }

  [Parameter] public IEnumerable<(MethodInfo Method, GridActionAttribute Attribute)>? GridActions { get; set; } = [];

  [Parameter] public ObservableCollection<T> Selected { get; set; } = [];

  [Parameter] public EventCallback<ObservableCollection<T>> SelectedChanged { get; set; }

  [Parameter] public Action? RefreshData { get; set; }

  [Parameter] public virtual Type? EditFormType { get; set; }

  [Parameter] public Action<T?>? OnSave { get; set; }

  [Parameter] public Action<T>? OnDiscard { get; set; }

  [Parameter] public Func<T>? OnNewItem { get; set; }

  [Parameter] public bool ShowExportToExcel { get; set; } = true;
  
  [Parameter] public bool UseVirtualization { get; set; } = false;
}