namespace BlazorGenerator.Components.Card
{
  public partial class CardFields<T>
  {
    protected void HandleSave(T Data)
    {
      OnSave?.Invoke(Data);
    }

    protected void HandleDiscard(T Data)
    {
      OnDiscard?.Invoke(Data);
    }
  }
}
