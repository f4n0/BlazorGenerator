using BlazorEngine.Security;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorEngine.Menu;

public sealed record MenuItemDefinition(
  Type PageType,
  string Title,
  string Route,
  PermissionFlags Permissions,
  Icon Icon,
  int Order,
  string Group);

public sealed record FooterItemDefinition(string Title, string Route, Icon? Icon, bool OpenInNewTab);
