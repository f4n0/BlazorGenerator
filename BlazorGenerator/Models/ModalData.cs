using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Models
{
  public class ModalData<T>
  {
    public required T Data { get; set; }
    public required List<VisibleField<T>> VisibleFields { get; set; }
  }
}
