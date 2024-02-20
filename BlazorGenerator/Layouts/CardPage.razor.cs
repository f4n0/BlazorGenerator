using BlazorGenerator.Components.Base;
using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator.Layouts
{
  public partial class CardPage<T> : BlazorgenComponentBase, IDialogContentComponent<T>
  {
    public virtual int GridSize => 6;

    [Parameter]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public T Content { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public List<VisibleField<T>> VisibleFields { get; set; } = [];

    public virtual void OnSave(T entity)
    {
    }

    public virtual void OnDelete(T entity)
    {
    }
  }
}
