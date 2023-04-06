using Blazorise.DataGrid;
using BlazorGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BlazorGenerator.Infrastructure;
using System.Threading.Tasks;

namespace BlazorGenerator.Pages
{
  partial class WorksheetPage<T, TList> : BlazorgenBaseComponent
  {
    bool FieldVisible { get;set; } = true;

    public List<TList> SelectedRecs { get; private set; } = new List<TList>();
    public List<TList> ListData { get; set; }
    public List<VisibleField<TList>> ListVisibleFields { get; set; } = new List<VisibleField<TList>>();
    private DataGrid<TList> _datagrid;

    public T Data { get; set; }
    public List<VisibleField<T>> VisibleFields { get; set; } = new List<VisibleField<T>>();

    public void Refresh()
    {
      _datagrid.Reload();
      _datagrid.Refresh();
    }

    public virtual TList CreateNewItem()
    {
      return Expression.Lambda<Func<TList>>(Expression.New(typeof(TList))).Compile().Invoke();
    }


    public virtual void OnInsert(SavedRowItem<TList, Dictionary<string, object>> e)
    {
    }

    public virtual void OnModify(SavedRowItem<TList, Dictionary<string, object>> e)
    {
    }

    public virtual void OnDelete(TList model)
    {
    }

    public virtual Action<TList, DataGridRowStyling> RowStyling => null;

    internal DataGridEditMode GetEditMode()
    {
      if (ListVisibleFields.Any(o => o.EditOnly))
      {
        return DataGridEditMode.Form;
      }
      else
      {
        return DataGridEditMode.Inline;
      }
    }

    public virtual object GroupBy(TList item) => null;
    public virtual bool GroupByEnabled() => false;

    private bool OnCustomFilter(TList model)
    {

      // We want to accept empty value as valid or otherwise
      // datagrid will not show anything.
      if (this.DataGridCustomFilter.Count == 0)
        return true;

      bool filter = false;

      foreach (var item in this.DataGridCustomFilter)
      {
        var fldVal = (item.Key as VisibleField<T>).Getter(model)?.ToString();
        filter = filter || fldVal.Contains(item.Value, StringComparison.OrdinalIgnoreCase) == true;
      }
      return filter;
    }
  }
}