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
    public partial class Worksheet<TData, TList>: BlazorgenComponentBase
    {

        private TData OriginalData { get; set; }
        private TData _data;
        public TData Data
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



        public IQueryable<TList> ListData { get; set; }

        public List<VisibleField<TList>> ListVisibleFields { get; set; } = new List<VisibleField<TList>>();


        private async Task EditAsync(TList context)
        {
            var panelData = new ModalData<TList>()
            {
                Data = context,
                VisibleFields = ListVisibleFields,
            };
            var _dialog = await UIServices.dialogService.ShowPanelAsync<ListEditPanel<TList>>(panelData, new DialogParameters()
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
                ListSave((result.Data as ModalData<TList>).Data, context);
            }
        }
        public virtual void ListDelete(TList context)
        {
            throw new NotImplementedException("Views must implement Save and Delete virtual methods");
        }

        public virtual void ListSave(TList Rec, TList xRec)
        {
            throw new NotImplementedException("Views must implement Save and Delete virtual methods");
        }
    }
}
