using System.Collections.ObjectModel;
using BlazorGenerator.Components.Base;
using BlazorGenerator.Components.Card;
using BlazorGenerator.Components.DataGrid;
using BlazorGenerator.Models;

namespace BlazorGenerator.Layouts
{
  public partial class Worksheet<TData, TList> : BlazorGeneratorComponentBase where TList : class
  {
    public virtual int GridSize => 6;
    public TData? Content { get; set; }
    public List<VisibleField<TData>> VisibleFields { get; set; } = [];

    public IEnumerable<TList>? ListContent { get; set; }
    public List<VisibleField<TList>> ListVisibleFields { get; set; } = [];
    public virtual Type? ListEditFormType { get; set; }

    public ObservableCollection<TList> ListSelected { get; set; } = [];
    
    private CardFields<TData>? Card { get; set; }
    private ListDataGrid<TList>? List { get; set; }

    public virtual void OnSave(TList entity)
    {
    }

    public virtual void OnDelete(TList entity)
    {
    }

    public virtual void OnSave(TData entity)
    {
    }

    public virtual void OnDelete(TData entity)
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
    
    
    public void RefreshCard()
    {
      if (Card != null)
      {
        Card.Data = Content;
        Card.VisibleFields = VisibleFields;
        Card.Refresh();
      }
    }
    public void RefreshList()
    {
      if (List != null)
      {
        List.Data = ListContent?.AsQueryable();
        List.VisibleFields = ListVisibleFields;
        List.Refresh();
      }
    }
  }
}