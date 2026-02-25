using BlazorEngine.Components.Base;
using BlazorEngine.Components.Card;
using BlazorEngine.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine.Layouts;

public partial class CardPage<T> : BlazorEngineComponentBase, IDialogContentComponent<T>
{
  private PermissionSet permissionSet;
  public virtual int GridSize => 6;

  public List<VisibleField<T>> VisibleFields { get; set; } = [];

  private CardFields<T>? Card { get; set; }

  [Parameter] public required T Content { get; set; }

  public virtual void OnSave(T entity)
  {
  }

  public virtual void OnDelete(T entity)
  {
  }

  protected override async Task OnInitializedAsync()
  {
    permissionSet = await Security.GetPermissionSet(GetType());
    await base.OnInitializedAsync();
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

  public void Refresh()
  {
    if (Card != null)
    {
      Card.Data = Content;
      Card.VisibleFields = VisibleFields;
      Card.Refresh();
    }
  }
}