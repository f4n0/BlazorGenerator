namespace BlazorGenerator.Components.Card
{
  public partial class CardFields<T>
  {
    bool _ShowAdditional { get; set; } = false;
    protected void HandleSave(T Data)
    {
      OnSave?.Invoke(Data);
    }

    protected void HandleDiscard(T Data)
    {
      OnDiscard?.Invoke(Data);
    }

    protected void ShowAdditionalFields()
    {
      _ShowAdditional = !_ShowAdditional;
    }
  }
}
