using Blazorise;
using Blazorise.DataGrid;
using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BlazorGenerator.Infrastructure;

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

    public virtual object ListGroupBy(TList item) => new { };
    
  }
}