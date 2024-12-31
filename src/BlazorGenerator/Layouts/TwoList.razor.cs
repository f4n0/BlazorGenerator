using BlazorGenerator.Components.Base;
using BlazorGenerator.Models;

namespace BlazorGenerator.Layouts
{
  public partial class TwoList<TFirstList, TSecondList> : BlazorGeneratorComponentBase
    where TFirstList : class
    where TSecondList : class
  {
    public IEnumerable<TFirstList>? FirstListContent { get; set; }
    public required List<VisibleField<TFirstList>> FirstListVisibleFields { get; set; }
    public virtual Type? FirstListEditFormType { get; set; }
    public List<TFirstList> FirstListSelected { get; set; } = [];

    public IEnumerable<TSecondList>? SecondListContent { get; set; }
    public required List<VisibleField<TSecondList>> SecondListVisibleFields { get; set; }
    public virtual Type? SecondListEditFormType { get; set; }
    public List<TSecondList> SecondListSelected { get; set; } = [];

    public virtual void OnSave(TFirstList entity)
    {
    }

    public virtual void OnDelete(TFirstList entity)
    {
    }

    public virtual void OnSave(TSecondList entity)
    {
    }

    public virtual void OnDelete(TSecondList entity)
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
