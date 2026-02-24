using BlazorEngine.Attributes;
using Microsoft.FluentUI.AspNetCore.Components;
using static Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20;

namespace BlazorEngine.Components.Menu
{
  public partial class NavMenu
  {
    private bool _expanded = true;
    private bool _loading = false;

    // Cached icon
    private static readonly Icon HomeIcon = new Home();

    // Pre-computed menu structure
    private List<AddToMenuAttribute> _defaultMenus = [];
    private List<MenuGroupData> _nonDefaultGroups = [];
    private FooterLinkAttribute? _footerLink;

    // Helper class to hold pre-computed group data
    private class MenuGroupData
    {
      public required string GroupName { get; init; }
      public required List<AddToMenuAttribute> Items { get; init; }
      public bool IsSingleItem => Items.Count == 1;
    }

    protected override void OnInitialized()
    {
      _loading = true;
      _ = BuildMenuStructureAsync();
    }

    private async Task BuildMenuStructureAsync()
    {
      try
      {
        var allMenu = Services.MetadataRegistry.GetMenuItems().ToList();
        if (allMenu.Count == 0)
        {
          return;
        }
        // Optional: parallelize permission checks (with a cap)
        // 2. Batch permission check
        var permissions = await Security.GetPermissionSets( allMenu.Select(m => m.Type).ToList()); // returns Dictionary<Type, PermissionSet>

        // 3. Build results using the batch permissions
        var authorizedMenus = allMenu
          .Select(item => (item, allowed: permissions.TryGetValue(item.Type, out var perm) && perm.Execute))
          .Where(r => r.allowed)
          .Select(r => (Attr: r.item.Attribute, Group: r.item.Attribute.Group))
          .ToList();

        // Pre-compute default group menus (sorted)
        _defaultMenus = authorizedMenus
          .Where(m => m.Group.Equals("default", StringComparison.OrdinalIgnoreCase))
          .Select(m => m.Attr)
          .Distinct()
          .OrderBy(m => m.OrderSequence)
          .ToList();

        // Pre-compute non-default groups with their items (sorted)
        _nonDefaultGroups = authorizedMenus
          .Where(m => !m.Group.Equals("default", StringComparison.OrdinalIgnoreCase))
          .GroupBy(m => m.Group)
          .Select(g => new MenuGroupData
          {
            GroupName = g.Key,
            Items = g.Select(x => x.Attr)
              .Distinct()
              .OrderBy(x => x.OrderSequence)
              .ToList()
          })
          .ToList();

        // Footer link
        var firstFooter = Services.MetadataRegistry.GetFooterLinks().FirstOrDefault();
        _footerLink = firstFooter.Attribute;
      }
      finally
      {
        _loading = false;
        await InvokeAsync(StateHasChanged);
      }
    }
  }
}