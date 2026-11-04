using BlazorEngine.Components.Base;
using BlazorEngine.Menu;
using BlazorEngine.Security;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Filled;

namespace BlazorEngine;

/// <summary>Explicit page registrations. Blazor's Router remains responsible for routes.</summary>
public sealed class BlazorEngineOptions
{
  private readonly Dictionary<Type, MenuItemDefinition> _menuItems = [];
  private readonly List<FooterItemDefinition> _footerItems = [];

  // Keep registration order for equally ordered items, as in the original menu.
  public IReadOnlyCollection<MenuItemDefinition> MenuItems => _menuItems.Values.OrderBy(item => item.Order).ToArray();
  public IReadOnlyCollection<FooterItemDefinition> FooterItems => _footerItems.ToArray();

  public void AddMenuItem<TPage>(string title, string route,
    PermissionFlags permissions = PermissionFlags.All, Icon? icon = null, int order = 999, string? group = null)
    where TPage : BlazorEngineComponentBase
  {
    _menuItems[typeof(TPage)] = new MenuItemDefinition(typeof(TPage), title, route, permissions,
      icon ?? new Size20.Balloon(), order, group ?? "Default");
  }

  public void AddFooterItem(string title, string route, Icon? icon = null, bool openInNewTab = false)
  {
    _footerItems.Add(new FooterItemDefinition(title, route, icon, openInNewTab));
  }

  internal PermissionFlags GetPermissions(Type? pageType) =>
    pageType is not null && _menuItems.TryGetValue(pageType, out var item) ? item.Permissions : PermissionFlags.All;
}
