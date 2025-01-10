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
    public Action? OnFirstListSelectedChanged { get; set; }
    private List<TFirstList> _firstListSelected = [];
    public List<TFirstList> FirstListSelected
    {
      get => _firstListSelected;
      set
      {
        _firstListSelected = value;
        OnFirstListSelectedChanged?.Invoke();
      }
    }

    public IEnumerable<TSecondList>? SecondListContent { get; set; }
    public required List<VisibleField<TSecondList>> SecondListVisibleFields { get; set; }
    public virtual Type? SecondListEditFormType { get; set; }
    public Action? OnSecondListSelectedChanged { get; set; }
    private List<TSecondList> _secondListSelected = [];
    public List<TSecondList> SecondListSelected
    {
      get => _secondListSelected;
      set
      {
        _secondListSelected = value;
        OnSecondListSelectedChanged?.Invoke();
      }
    }


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
