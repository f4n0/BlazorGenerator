﻿using BlazorGenerator.Components.Base;
using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator.Layouts
{
  public partial class CardPage<T> : BlazorGeneratorComponentBase, IDialogContentComponent<T>
  {
    public virtual int GridSize => 6;

    [Parameter]
    public required T Content { get; set; }

    public List<VisibleField<T>> VisibleFields { get; set; } = [];

    public virtual void OnSave(T entity)
    {
    }

    public virtual void OnDelete(T entity)
    {
    }

    public override void InternalDispose()
    {
      GC.SuppressFinalize(this);
      base.InternalDispose();
    }

    public override ValueTask InternalDisposeAsync()
    {
      GC.SuppressFinalize(this);
      return base.InternalDisposeAsync();
    }
  }
}
