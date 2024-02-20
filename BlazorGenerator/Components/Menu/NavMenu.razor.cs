using BlazorGenerator.Attributes;

namespace BlazorGenerator.Components.Menu
{
  public partial class NavMenu
  {
    Dictionary<string, int> MenuGroups { get; set; } = [];

    List<AddToMenuAttribute> Menus { get; set; } = [];

    protected override async Task OnParametersSetAsync()
    {
      await PopulateDictionaryAsync();
      await base.OnParametersSetAsync();
    }

    async Task PopulateDictionaryAsync()
    {
      Menus.Clear();
      var allMenu = Utils.AttributesUtils.GetModelsWithAttribute<AddToMenuAttribute>();
      MenuGroups = [];
      foreach (var item in allMenu)
      {
        if ((await Security?.GetPermissionSet(item.Type)).Execute)
        {
          if (!Menus.Contains(item.Attribute))
            Menus.Add(item.Attribute);
          if (MenuGroups.TryGetValue(item.Attribute.Group, out int value))
          {
            MenuGroups[item.Attribute.Group] = ++value;
          }
          else
          {
            MenuGroups.Add(item.Attribute.Group, 1);
          }
        }
      }
    }
  }
}
