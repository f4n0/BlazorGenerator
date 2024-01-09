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
  public partial class ListPage<T> : BlazorgenComponentBase
  {
    public IQueryable<T> Data { get; set; }
    public List<T> Selected { get; set; } = new List<T>();
    internal T CurrRec { get; set; }
    private bool MenuOpen;

    public List<VisibleField<T>> VisibleFields { get; set; } = new List<VisibleField<T>>();

    private async Task EditAsync(T context)
    {
      var panelData = new ModalData<T>()
      {
        Data = context,
        VisibleFields = VisibleFields,
      };
      var _dialog = await UIServices.dialogService.ShowPanelAsync<ListEditPanel<T>>(panelData, new DialogParameters()
      {
        DialogType = DialogType.Panel,
        Alignment = HorizontalAlignment.Right,
        Width = "40%"


      });
      DialogResult result = await _dialog.Result;

      if (result.Cancelled)
      {
        // do nothing
      }
      if (result.Data is not null)
      {
        Save((result.Data as ModalData<T>).Data, context);
      }
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
