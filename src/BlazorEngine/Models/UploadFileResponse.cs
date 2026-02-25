namespace BlazorEngine.Models;

public class UploadFileResponse
{
  public string FileName { get; set; } = string.Empty;
  public byte[]? Data { get; set; }
  public string? ErrorMessage { get; set; }

  public async Task FromStreamAsync(Stream input, CancellationToken ct = default)
  {
    using MemoryStream ms = new();
    await input.CopyToAsync(ms, ct).ConfigureAwait(true);
    Data = ms.ToArray();
  }
}