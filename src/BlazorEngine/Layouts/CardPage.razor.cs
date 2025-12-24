using BlazorEngine.Components.Base;
using BlazorEngine.Components.Card;
using BlazorEngine.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine.Layouts
{
  public partial class CardPage<T> : BlazorEngineComponentBase, IDialogContentComponent<T>
  {
    public virtual int GridSize => 6;

    [Parameter]
    public required T Content { get; set; }

    public List<VisibleField<T>> VisibleFields { get; set; } = [];

    private CardFields<T>? Card { get; set; }

    public virtual void OnSave(T entity)
    {
    }

    public virtual void OnDelete(T entity)
    {
    }

    private PermissionSet permissionSet;

    protected override async Task OnInitializedAsync()
    {
      permissionSet = await Security.GetPermissionSet(this.GetType());
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
}
