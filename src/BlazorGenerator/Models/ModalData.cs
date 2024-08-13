namespace BlazorGenerator.Models
{
  public class ModalData<T>
  {
    public required T Data { get; set; }
    public required List<VisibleField<T>> VisibleFields { get; set; }
  }
}
