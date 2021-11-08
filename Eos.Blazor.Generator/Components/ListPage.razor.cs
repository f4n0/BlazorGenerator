using Eos.Blazor.Generator.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eos.Blazor.Generator.Components
{
  partial class ListPage<T> : ComponentBase
  {
    public List<T> SelectedRecs { get; private set; } = new List<T>();
    public List<T> Data { get; set; }
    public List<VisibleField<T>> VisibleFields { get; set; } = new List<VisibleField<T>>();
    public bool isEditable = false;

    void Edit()
    {
      isEditable = !isEditable;
    }

  }
}
