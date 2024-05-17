using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator.Components.Modals
{
  public partial class FileInput : IDialogContentComponent<UploadFileData>
  {
    [Parameter]
    public UploadFileData Content { get; set; } = new();
    int ProgressPercent = 0;

    [CascadingParameter]
    public FluentDialog? Dialog { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0305:Simplify collection initialization", Justification = "<Pending>")]
    private async Task OnCompletedAsync(IEnumerable<FluentInputFileEventArgs> files)
    {
      var converted = new List<UploadFileResponse>();
      foreach (var file in files)
      {
        var res = new UploadFileResponse()
        {
          FileName = file.Name
        };
        if (file.Stream != null)
        {
          await res.FromStreamAsync(file.Stream);
        }
        converted.Add(res);
      }

      Content.Files = converted.ToArray();

      await Task.Delay(500);
      ProgressPercent = 0;
      await Dialog!.CloseAsync(Content);
    }
  }
}
