using BlazorGenerator.Components.Base;
using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Layouts
{
    public partial class CardPage<T> : BlazorgenComponentBase
    {

        private T OriginalData { get; set; }
        private T _data;
        public T Data
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

        public List<VisibleField<T>> VisibleFields { get; set; } = new List<VisibleField<T>>();

        public virtual void Save(T Rec, T xRec)
        {

        }
        public virtual void Discard(T Rec, T xRec)
        {

        }

        internal virtual int GridSize => 6;
    }
}
