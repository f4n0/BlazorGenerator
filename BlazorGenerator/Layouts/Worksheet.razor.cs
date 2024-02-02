using BlazorGenerator.Components.Base;
using BlazorGenerator.Layouts.Partial;
using BlazorGenerator.Models;
using BlazorGenerator.Utils;
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
    private TData? OriginalData { get; set; }
    private TData? _data;
    public TData Content
    {
      get
      {
        return _data!;
      }
      set
      {
        _data = value;
        OriginalData = value;
      }
    }

    public List<VisibleField<TData>> VisibleFields { get; set; } = [];

    void Save(TData Rec)
    {
      OnInsert(Rec);
      OnModify(Rec, OriginalData!);
    }
    void Discard(TData Rec)
    {
      OnDelete(Rec);
    }

    internal virtual int GridSize => 6;

    public IQueryable<TList>? ListContent { get; set; }

    public List<VisibleField<TList>> ListVisibleFields { get; set; } = [];
    public virtual Type? EditFormType { get; set; }

    public List<TList> ListSelected { get; set; } = [];
    internal TList? CurrRec { get; set; }

    private async Task EditAsync(TList context)
    {
      TList? res;
      if (EditFormType != null)
      {
        res = await UIServices!.OpenPanel<TList>(EditFormType, context);
      }
      else
      {
        var typeDelegate = RoslynUtilities.CreateAndInstatiateClass(VisibleFields, "edit");
        var type = (Type)typeDelegate.Invoke().Result;
        res = await UIServices!.OpenPanel<TList>(type, context);
        GC.Collect();
      }
      OnModify(res!, context);
    }
    void ListDelete(TList context)
    {
      OnDelete(context);
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

    public virtual void OnInsert(TList entity)
    {
    }

    public virtual void OnModify(TList entity, TList oldEntity)
    {
    }

    public virtual void OnDelete(TList entity)
    {
    }

    public virtual void OnInsert(TData entity)
    {
    }

    public virtual void OnModify(TData entity, TData oldEntity)
    {
    }

    public virtual void OnDelete(TData entity)
    {
    }
  }
}
