using BlazorGenerator.Dialogs;
using BlazorGenerator.Models;
using BlazorGenerator.Services;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Infrastructure
{
  public class BlazorgenBaseComponent : ComponentBase, IDisposable
  {

    [CascadingParameter]
    protected DynamicMainLayout layout { get; set; }
    [Inject]
    public UIServices UiServices { get; set; }

    [Inject]
    public IJSRuntime JSRuntime { get; set; }
    [Inject]
    public NavigationManager NavManager { get; set; }
    [Inject]
    public BlazorGenOptions Options { get; set; }


    public void setLogVisibility(bool show)
    {
      layout?.setLogVisibility(show);
    }

    public async Task<string> ShowChoose(string header, string[] options, string cancelText = "Cancel")
    {
      return await layout.ChooseService.ChoseAsync(header, options, cancelText);
    }

    public virtual string Title => this.GetType().Name;

    public void StartLoader()
    {
      UiServices.PageProgressService.Go(null, options => { options.Color = Color.Danger; });
    }
    public void StopLoader()
    {
      UiServices.PageProgressService.Go(-1);
    }




    #region Modal

    [Parameter]
    public bool IsModal { get; set; }
    [Parameter] public Func<object, Task> ModalSuccess { get; set; }
    

    protected override Task OnParametersSetAsync()
    {
      if (IsModal)
      {
        setLogVisibility(false);
      }
      return base.OnParametersSetAsync();
    }

    #endregion


    #region Files

    public Task<object> ShowFilePicker()
    {
      return UiServices.OpenModalAsync<UploadFileDialog>();
    }


    #endregion

    public void SendLogMessage(string message, Enum.LogType logType = Enum.LogType.Info)
    {
      UiServices.logger.SendLogMessage(message, logType);
    }

    public virtual void Dispose()
    {

    }

    internal Dictionary<object, string> DataGridCustomFilter { get; set; } = new Dictionary<object, string>();

    internal void AddFilter(object Field, string selectedFilter)
    {
      if (DataGridCustomFilter.ContainsKey(Field))
        DataGridCustomFilter[Field] = selectedFilter;
      else
        DataGridCustomFilter.Add(Field, selectedFilter);
    }
  }
}
