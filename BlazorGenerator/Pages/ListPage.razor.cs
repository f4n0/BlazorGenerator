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
  }
}
