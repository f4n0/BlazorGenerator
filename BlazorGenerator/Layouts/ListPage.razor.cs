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
      Save(res, context);
    }
    public virtual void Delete(T context)
    {
      throw new NotImplementedException("Views must implement Save and Delete virtual methods");
    }

    public virtual void Save(T Rec, T xRec)
    {
      throw new NotImplementedException("Views must implement Save and Delete virtual methods");
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
  }
}
