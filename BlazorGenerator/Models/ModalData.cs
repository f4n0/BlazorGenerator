using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Models
{
    public class ModalData<T>
    {
        public T Data { get; set; }
        public List<VisibleField<T>> VisibleFields { get; set; }
    }
}
