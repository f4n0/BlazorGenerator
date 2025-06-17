namespace BlazorEngine.Models
{
  public class UploadFileData
  {
    public bool Multiple { get; set; } = true;
    public string FileFilters { get; set; } = "*.*";
    public int MaximumFileCount { get; set; } = 50;
    public long MaximumFileSize { get; set; } = 10 * 1024 * 1024;
    public UploadFileResponse[] Files { get; set; } = [];

    public CancellationToken CancellationToken { get; set; } = default;
  }
}
