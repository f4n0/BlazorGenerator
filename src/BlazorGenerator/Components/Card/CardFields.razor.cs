namespace BlazorGenerator.Components.Card
{
  public partial class CardFields<T>
  {
    bool ShowAdditional { get; set; } = false;
    protected void HandleSave(T data)
    {
      OnSave?.Invoke(data);
    }

    protected void HandleDiscard(T data)
    {
      OnDiscard?.Invoke(data);
    }

    protected void ShowAdditionalFields()
    {
      ShowAdditional = !ShowAdditional;
    }

    internal void Refresh()
    {
      StateHasChanged();
    }
  }
}
