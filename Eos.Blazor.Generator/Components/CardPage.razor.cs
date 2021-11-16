using Eos.Blazor.Generator.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eos.Blazor.Generator.Components
{
  partial class CardPage<T> : ComponentBase
  {
    public T Data { get; set; }
    public List<VisibleField<T>> VisibleFields { get; set; } = new List<VisibleField<T>>();
  }
}
