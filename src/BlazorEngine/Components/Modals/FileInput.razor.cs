using System.Diagnostics.CodeAnalysis;
using BlazorEngine.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine.Components.Modals;

public partial class FileInput : FluentDialogInstance
{
  private int _progressPercent;

  [Parameter] public UploadFileData Content { get; set; } = new();

  [SuppressMessage("Style", "IDE0305:Simplify collection initialization", Justification = "<Pending>")]
  private async Task OnCompletedAsync(IEnumerable<FluentInputFileEventArgs> files)
  {
    var converted = new List<UploadFileResponse>();
    foreach (var file in files)
    {
      var res = new UploadFileResponse
      {
        FileName = file.Name,
        ErrorMessage = file.ErrorMessage
      };
      if (file.Stream != null) await res.FromStreamAsync(file.Stream, Content.CancellationToken);
      converted.Add(res);
    }

    Content.Files = converted.ToArray();

    await Task.Delay(500, Content.CancellationToken);
    _progressPercent = 0;
    await DialogInstance.CloseAsync(Content);
  }

  protected override Task OnActionClickedAsync(bool primary) =>
    primary ? DialogInstance.CloseAsync(Content) : DialogInstance.CancelAsync();
}
