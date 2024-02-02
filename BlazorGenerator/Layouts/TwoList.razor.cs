using BlazorGenerator.Components.Base;
using BlazorGenerator.Layouts.Partial;
using BlazorGenerator.Models;
using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Layouts
{
  public partial class TwoList<TFirstList, TSecondList> : BlazorgenComponentBase
    where TFirstList : class
    where TSecondList : class
  {
    public IQueryable<TFirstList>? FirstListContent { get; set; }
    public required List<VisibleField<TFirstList>> FirstListVisibleFields { get; set; }
    public virtual Type? FirstListEditFormType { get; set; }
    public List<TFirstList> FirstListSelected { get; set; } = [];

    public IQueryable<TSecondList>? SecondListContent { get; set; }
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
  }
}
