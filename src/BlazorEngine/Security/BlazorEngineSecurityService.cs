using System.Collections.Concurrent;
using BlazorEngine.Models;

namespace BlazorEngine.Security;

internal class BlazorEngineSecurityService(IServiceProvider services, BlazorEngineOptions options)
{
  private readonly ConcurrentDictionary<(string SessionId, Type? ObjectType), PermissionSet> _permissionCache = new();
  public ISecurity Security { get; set; } = (ISecurity)services.GetService(typeof(ISecurity))!;

  public async Task<PermissionSet> GetPermissionSet(Type? @object = null)
  {
    var sessionId = await Security.GetCurrentSessionIdentifier().ConfigureAwait(true);
    if (string.IsNullOrEmpty(sessionId))
      return ApplyRegisteredPermissions(@object, await Security.GetPermissionSet(@object).ConfigureAwait(true));

    if (_permissionCache.TryGetValue((sessionId, @object), out var permissionSet))
      return ApplyRegisteredPermissions(@object, permissionSet);

    if (@object != null && _permissionCache.TryGetValue((sessionId, null), out permissionSet))
      return ApplyRegisteredPermissions(@object, permissionSet);

    var result = await Security.GetPermissionSet(@object).ConfigureAwait(true);
    _permissionCache.TryAdd((sessionId, @object), result);
    return ApplyRegisteredPermissions(@object, result);
  }

  // Registration can restrict permissions, never grant permissions denied by ISecurity.
  // Do not mutate the cached session-wide or provider-owned permission set.
  private PermissionSet ApplyRegisteredPermissions(Type? pageType, PermissionSet granted)
  {
    var allowed = options.GetPermissions(pageType);
    return new PermissionSet
    {
      Object = granted.Object,
      Insert = granted.Insert && allowed.HasFlag(PermissionFlags.Insert),
      Modify = granted.Modify && allowed.HasFlag(PermissionFlags.Modify),
      Delete = granted.Delete && allowed.HasFlag(PermissionFlags.Delete),
      Execute = granted.Execute && allowed.HasFlag(PermissionFlags.Execute),
      RequireAuthentication = granted.RequireAuthentication
    };
  }

  public async Task<Dictionary<Type, PermissionSet>> GetPermissionSets(IEnumerable<Type> types)
  {
    var result = new Dictionary<Type, PermissionSet>();
    foreach (var type in types.Distinct())
    {
      // Assume GetPermissionSet is your existing method
      var permission = await GetPermissionSet(type);
      result[type] = permission;
    }

    return result;
  }

  public async Task<string> GetSessionIdentifier()
  {
    return await Security.GetCurrentSessionIdentifier().ConfigureAwait(true);
  }
}
