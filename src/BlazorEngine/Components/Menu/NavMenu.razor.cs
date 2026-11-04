using BlazorEngine.Menu;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using static Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20;

namespace BlazorEngine.Components.Menu;

public partial class NavMenu
{
  [Inject] private BlazorEngineOptions Options { get; set; } = null!;
  [Parameter] public bool Collapsible { get; set; } = true;
  [Parameter] public bool Expanded { get; set; } = true;
  [Parameter] public EventCallback<bool> ExpandedChanged { get; set; }

  private static readonly Icon CollapsedDefaultIcon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Document();

  private Task ToggleExpandedAsync() => SetExpandedAsync(!Expanded);

  private readonly Dictionary<string, bool> _expandedCategories = new(StringComparer.Ordinal);

  private Task HandleCategoryExpandedAsync(string name, bool expanded)
  {
    _expandedCategories[name] = expanded || !Expanded;
    return !Expanded ? SetExpandedAsync(true) : Task.CompletedTask;
  }

  private async Task SetExpandedAsync(bool expanded)
  {
    Expanded = expanded;
    await ExpandedChanged.InvokeAsync(expanded);
  }

  private Icon? GetNavIcon(Icon? icon) => icon ?? (Expanded ? null : CollapsedDefaultIcon);

  // Cached icon
  private static readonly Icon HomeIcon = new Home();

  // Pre-computed menu structure
  private List<MenuItemDefinition> _defaultMenus = [];
  private IReadOnlyCollection<FooterItemDefinition> _footerLinks = [];
  private bool _loading;
  private List<MenuGroupData> _nonDefaultGroups = [];

  protected override async Task OnInitializedAsync()
  {
    _loading = true;
    await BuildMenuStructureAsync();
    await base.OnInitializedAsync();
  }

  private async Task BuildMenuStructureAsync()
  {
    try
    {
      var allMenu = Options.MenuItems;
      // Preserve the host's session-aware ISecurity checks as well as registration restrictions.
      var permissions =
        await Security.GetPermissionSets(allMenu.Select(m => m.PageType));

      // Build results using the batch permissions.
      var authorizedMenus = allMenu
        .Select(item => (item, allowed: permissions.TryGetValue(item.PageType, out var perm) && perm.Execute))
        .Where(r => r.allowed)
        .Select(r => r.item)
        .ToList();

      // Pre-compute default group menus (sorted)
      _defaultMenus = authorizedMenus
        .Where(m => m.Group.Equals("default", StringComparison.OrdinalIgnoreCase))
        .Distinct()
        .OrderBy(m => m.Order)
        .ToList();

      // Pre-compute non-default groups with their items (sorted)
      _nonDefaultGroups = authorizedMenus
        .Where(m => !m.Group.Equals("default", StringComparison.OrdinalIgnoreCase))
        .GroupBy(m => m.Group)
        .Select(g => new MenuGroupData
        {
          GroupName = g.Key,
          Items = g
            .Distinct()
            .OrderBy(x => x.Order)
            .ToList()
        })
        .ToList();

      // Footer registrations are independent of page/menu permissions.
      _footerLinks = Options.FooterItems;
    }
    finally
    {
      _loading = false;
      await InvokeAsync(StateHasChanged);
    }
  }

  // Helper class to hold pre-computed group data
  private class MenuGroupData
  {
    public required string GroupName { get; init; }
    public required List<MenuItemDefinition> Items { get; init; }
    public bool IsSingleItem => Items.Count == 1;
  }
}
