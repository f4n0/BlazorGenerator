namespace BlazorGenerator.Models
{
  public class UploadFileData
  {
    public bool Multiple { get; set; } = true;
    public string FileFilters { get; set; } = "*.*";
    public int MaximumFileCount { get; set; } = 50;
    public UploadFileResponse[] Files { get; set; }
  }

  public class UploadFileResponse
  {
    public string FileName { get; set; }
    public byte[] Data { get; set; }

    public async Task fromStreamAsync(Stream input)
    {

      using (MemoryStream ms = new MemoryStream())
      {
        await input.CopyToAsync(ms);
        Data = ms.ToArray();


      }

    }
  }
}
