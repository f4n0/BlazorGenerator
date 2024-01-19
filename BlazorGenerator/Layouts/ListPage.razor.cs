using BlazorGenerator.Components.Base;
using BlazorGenerator.Layouts.Partial;
using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Layouts
{
  public partial class ListPage<T> : BlazorgenComponentBase where T : class
  {
    public IQueryable<T> Content { get; set; }
    public List<VisibleField<T>> VisibleFields { get; set; } = new List<VisibleField<T>>();
    public virtual Type EditFormType { get; set; }

    public List<T> Selected { get; set; } = new List<T>();
    internal T CurrRec { get; set; }



    private async Task EditAsync(T context)
    {
      if (EditFormType  == null)
         throw new NotImplementedException("In order to use Edit action, you must implement EditFormType Property");
      var res = await UIServices.OpenPanel<T>(EditFormType, context);
      OnModify(res, context);
    }


    private void HandleRecSelection(bool selected, T Rec)
    {
      if (selected)
      {
        Selected.Add(Rec);
      } else
      {
        Selected.Remove(Rec);
      }
    }

    public void OnInsert(T entity)
    {
      throw new NotImplementedException();
    }

    public void OnModify(T entity, T oldEntity)
    {
      throw new NotImplementedException();
    }

    public void OnDelete(T entity)
    {
      throw new NotImplementedException();
    }
  }
}
