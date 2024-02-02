using BlazorGenerator.Attributes;
using BlazorGenerator.Components.Base;
using BlazorGenerator.Models;
using BlazorGenerator.Services;
using BlazorGenerator.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Layouts.Partial
{
  public partial class ListDataGrid<T> where T : class
  {
    [Inject]
    public UIServices? UIServices { get; set; }

    [Parameter]
    public List<VisibleField<T>> VisibleFields { get; set; } = [];

    [Parameter]
    public IQueryable<T>? Data { get; set; }

    [Parameter]
    public bool ShowButtons { get; set; }

    [Parameter]
    public PermissionSet? PermissionSet { get; set; }

    [Parameter]
    public IEnumerable<(MethodInfo Method, GridActionAttribute Attribute)>? GridActions { get; set; } = [];

    [Parameter]
    public List<T> Selected { get; set; } = [];

    [Parameter]
    public EventCallback<List<T>> SelectedChanged { get; set; }

    internal T? CurrRec { get; set; }

    [Parameter]
    public virtual Type? EditFormType { get; set; }

    private async Task EditAsync(T context)
    {
      T? res;
      if (EditFormType != null)
      {
        res = await UIServices!.OpenPanel<T>(EditFormType, context);
      }
      else
      {
        var typeDelegate = RoslynUtilities.CreateAndInstatiateClass(VisibleFields, "edit");
        var type = (Type)typeDelegate.Invoke().Result;
        res = await UIServices!.OpenPanel<T>(type, context);
        GC.Collect();
      }
      HandleSave(res!);
    }

    internal Func<T, string> SelectedRowClass => (data) => Selected.Contains(data) ? "rowselected" : "";

    private void HandleRecSelection(bool selected, T Rec)
    {
      if (selected)
      {
        Selected.Add(Rec);
      }
      else
      {
        Selected.Remove(Rec);
      }
    }

    [Parameter]
    public Action<T>? OnSave { get; set; }

    protected void HandleSave(T Data)
    {
      OnSave?.Invoke(Data);
    }

    [Parameter]
    public Action<T>? OnDiscard { get; set; }

    protected void HandleDiscard(T Data)
    {
      OnDiscard?.Invoke(Data);
    }

    static string GetCssGridTemplate(int GridActions, PermissionSet permissionSet)
    {
      const string select = "50px ";
      string actions = string.Empty;
      if (GridActions > 0)
        actions = "50px ";

      const string cols = "repeat(auto-fill,14%) ";
      string rowActions = string.Empty;
      if (permissionSet.Modify || permissionSet.Delete)
        rowActions = "150px";

      return select + actions + cols + rowActions;
    }
  }
}
