namespace BlazorGenerator.Models
{
  public class UploadFileData
  {
    public bool Multiple { get; set; } = true;
    public string FileFilters { get; set; } = "*.*";
    public int MaximumFileCount { get; set; } = 50;
    public UploadFileResponse[] Files { get; set; } = [];

    public CancellationToken CancellationToken { get; set; } = default;
  }

  public class UploadFileResponse
  {
    public string FileName { get; set; } = string.Empty;
    public byte[]? Data { get; set; }

    public async Task FromStreamAsync(Stream input, CancellationToken ct = default)
    {
      using MemoryStream ms = new();
      await input.CopyToAsync(ms,ct);
      Data = ms.ToArray();
    }
  }
}
