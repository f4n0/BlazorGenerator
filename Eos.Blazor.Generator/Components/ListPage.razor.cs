using Blazorise;
using Blazorise.DataGrid;
using Eos.Blazor.Generator.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eos.Blazor.Generator.Components
{
  partial class ListPage<T> : ComponentBase
  {
    [Inject] public IPageProgressService PageProgressService { get; set; }
    [Inject] public IMessageService MessageService { get; set; }

    public List<T> SelectedRecs { get; private set; } = new List<T>();
    public List<T> Data { get; set; }
    public List<VisibleField<T>> VisibleFields { get; set; } = new List<VisibleField<T>>();
    private DataGrid<T> _datagrid;


    public void StartLoader()
    {
      PageProgressService.Go(null, options => { options.Color = Color.Danger; });
    }
    public void StopLoader()
    {
      PageProgressService.Go(-1);
    }

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

    internal DataGridEditMode GetEditMode()
    {
      if(VisibleFields.Any(o => o.EditOnly))
      {
        return DataGridEditMode.Form;
      } else
      {
        return DataGridEditMode.Inline;
      }
    }
  }
}
