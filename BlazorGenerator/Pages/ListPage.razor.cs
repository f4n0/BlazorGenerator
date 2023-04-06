using Blazorise.DataGrid;
using BlazorGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BlazorGenerator.Infrastructure;

namespace BlazorGenerator.Pages
{
  partial class ListPage<T> : BlazorgenBaseComponent
  {
    public List<T> SelectedRecs { get; private set; } = new List<T>();
    public T SelectedRec { get; private set; }
    public List<T> Data { get; set; }
    public List<VisibleField<T>> VisibleFields { get; set; } = new List<VisibleField<T>>();
    private DataGrid<T> _datagrid;
    public virtual DataGridSelectionMode SelectionMode => DataGridSelectionMode.Multiple;

    public void Refresh()
    {
      _datagrid.Reload();
    }

    public virtual T CreateNewItem()
    {
      return Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile().Invoke();
    }

    public virtual void OnInsert(SavedRowItem<T, Dictionary<string, object>> e)
    {
    }

    public virtual void OnModify(SavedRowItem<T, Dictionary<string, object>> e)
    {
    }

    public virtual void OnDelete(T model)
    {
    }


    public virtual Action<T, DataGridRowStyling> RowStyling => null;

    internal DataGridEditMode GetEditMode()
    {
      if (VisibleFields.Any(o => o.EditOnly))
      {
        return DataGridEditMode.Form;
      }
      else
      {
        return DataGridEditMode.Inline;
      }
    }

    // public virtual object GrouppedBy(T item);


    public virtual object GroupBy(T item) => null;
    public virtual bool GroupByEnabled() => false;


    private bool OnCustomFilter(T model)
    {
      
      // We want to accept empty value as valid or otherwise
      // datagrid will not show anything.
      if (this.DataGridCustomFilter.Count == 0)
        return true;

      bool filter = false; 

      foreach (var item in this.DataGridCustomFilter)
      {
        var fldVal = (item.Key as VisibleField<T>).FilterGetter(model)?.ToString();
        filter = filter || fldVal.Contains(item.Value, StringComparison.OrdinalIgnoreCase) == true;
      }
      return filter;
    }
  }
}
