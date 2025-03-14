﻿using BlazorEngine.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorEngine.Components.Card;

public partial class CardFields<T>
{
  [Parameter] public List<VisibleField<T>> VisibleFields { get; set; } = [];

  [Parameter] public int GridSize { get; set; }

  [Parameter] public T? Data { get; set; }

  [Parameter] public bool ShowButtons { get; set; }

  [Parameter] public PermissionSet? PermissionSet { get; set; }

  [Parameter] public Action<T>? OnSave { get; set; }

  [Parameter] public Action<T>? OnDiscard { get; set; }
}