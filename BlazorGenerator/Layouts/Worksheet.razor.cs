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
  public partial class Worksheet<TData, TList> : BlazorgenComponentBase where TList : class
  {

    private TData OriginalData { get; set; }
    private TData _data;
    public TData Content
    {
      get
      {
        return _data;
      }
      set
      {
        _data = value;
        OriginalData = value;
      }
    }

    public List<VisibleField<TData>> VisibleFields { get; set; } = new List<VisibleField<TData>>();

    public virtual void Save(TData Rec, TData xRec)
    {

    }
    public virtual void Discard(TData Rec, TData xRec)
    {

    }

    internal virtual int GridSize => 6;



    public IQueryable<TList> ListContent { get; set; }

    public List<VisibleField<TList>> ListVisibleFields { get; set; } = new List<VisibleField<TList>>();
    public virtual Type EditFormType { get; set; }

    public List<TList> ListSelected { get; set; } = new List<TList>();
    internal TList CurrRec { get; set; }

    private async Task EditAsync(TList context)
    {
      if (EditFormType == null)
        throw new NotImplementedException("In order to use Edit action, you must implement EditFormType Property");
      var res = await UIServices.OpenPanel<TList>(EditFormType, context);
      ListSave(res, context);
    }
    public virtual void ListDelete(TList context)
    {
      throw new NotImplementedException("Views must implement Save and Delete virtual methods");
    }

    public virtual void ListSave(TList Rec, TList xRec)
    {
      throw new NotImplementedException("Views must implement Save and Delete virtual methods");
    }

    private void HandleRecSelection(bool selected, TList Rec)
    {
      if (selected)
      {
        ListSelected.Add(Rec);
      }
      else
      {
        ListSelected.Remove(Rec);
      }
    }
  }
}
