using Blazorise;
using Blazorise.DataGrid;
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
    [Inject] 
    public IPageProgressService PageProgressService { get; set; }
    public List<T> SelectedRecs { get; private set; } = new List<T>();
    public List<T> Data { get; set; }
    public List<VisibleField<T>> VisibleFields { get; set; } = new List<VisibleField<T>>();
    public bool isEditable = false;
    private DataGrid<T> _datagrid;

    void Edit()
    {
      isEditable = !isEditable;
    }

    public void StartLoader()
    {
      PageProgressService.Go(null, options => { options.Color = Color.Danger; });
    }
    public void StopLoader()
    {
      PageProgressService.Go(-1);
    }

    public void Refresh()
    {
      _datagrid.Reload();
    }
  }
}
