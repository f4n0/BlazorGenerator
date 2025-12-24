using BlazorEngine.Models;

namespace BlazorEngine.Components.Card
{
  public partial class CardFields<T>
  {
    private ILookup<string, VisibleField<T>>? _groupedFields;
    private string[]? _groups;

    protected override void OnParametersSet()
    {
      _groupedFields = VisibleFields.ToLookup(f => f.Group ?? string.Empty);
      _groups = _groupedFields.Where(g => g.Key != string.Empty).Select(g => g.Key).ToArray();
      base.OnParametersSet();
    }
    
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
