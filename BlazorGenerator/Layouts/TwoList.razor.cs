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
  {
    public IQueryable<TFirstList>? FirstListContent { get; set; }
    public IQueryable<TSecondList>? SecondListContent { get; set; }

    public List<VisibleField<TFirstList>> FirstListVisibleFields { get; set; } = [];
    public List<VisibleField<TSecondList>> SecondListVisibleFields { get; set; } = [];

    public List<TFirstList> FirstListSelected { get; set; } = [];
    public List<TSecondList> SecondListSelected { get; set; } = [];

    internal TFirstList? FirstListCurrRec { get; set; }
    internal TSecondList? SecondListCurrRec { get; set; }

    private async Task FirstListEditAsync(TFirstList context)
    {
      var panelData = new ModalData<TFirstList>()
      {
        Data = context,
        VisibleFields = FirstListVisibleFields,
      };
      var _dialog = await UIServices!.DialogService.ShowPanelAsync<ListEditPanel<TFirstList>>(panelData, new DialogParameters()
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
        FirstListSave((result.Data as ModalData<TFirstList>)!.Data, context);
      }
    }
    public virtual void FirstListDelete(TFirstList context)
    {
      throw new NotImplementedException("Views must implement Save and Delete virtual methods");
    }

    public virtual void FirstListSave(TFirstList Rec, TFirstList xRec)
    {
      throw new NotImplementedException("Views must implement Save and Delete virtual methods");
    }

    private async Task SecondListEditAsync(TSecondList context)
    {
      var panelData = new ModalData<TSecondList>()
      {
        Data = context,
        VisibleFields = SecondListVisibleFields,
      };
      var _dialog = await UIServices!.DialogService.ShowPanelAsync<ListEditPanel<TSecondList>>(panelData, new DialogParameters()
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
        SecondListSave((result.Data as ModalData<TSecondList>)!.Data, context);
      }
    }
    public virtual void SecondListDelete(TSecondList context)
    {
    }

    public virtual void SecondListSave(TSecondList Rec, TSecondList xRec)
    {
    }

    private void HandleRecSelection(bool selected, TFirstList Rec)
    {
      if (selected)
      {
        FirstListSelected.Add(Rec);
      }
      else
      {
        FirstListSelected.Remove(Rec);
      }
    }
    private void HandleRecSelection(bool selected, TSecondList Rec)
    {
      if (selected)
      {
        SecondListSelected.Add(Rec);
      }
      else
      {
        SecondListSelected.Remove(Rec);
      }
    }
  }
}
