using System.Collections.Concurrent;
using BlazorEngine.Models;

namespace BlazorEngine.Security;

internal class BlazorEngineSecurityService(IServiceProvider services)
{
  private readonly ConcurrentDictionary<(string SessionId, Type? ObjectType), PermissionSet> _permissionCache = new();
  public ISecurity Security { get; set; } = (ISecurity)services.GetService(typeof(ISecurity))!;

  public async Task<PermissionSet> GetPermissionSet(Type? @object = null)
  {
    var sessionId = await Security.GetCurrentSessionIdentifier().ConfigureAwait(true);
    if (string.IsNullOrEmpty(sessionId)) return await Security.GetPermissionSet(@object).ConfigureAwait(true);

    if (_permissionCache.TryGetValue((sessionId, @object), out var permissionSet))
      return permissionSet;

    if (@object != null && _permissionCache.TryGetValue((sessionId, null), out permissionSet))
      return permissionSet;

    var result = await Security.GetPermissionSet(@object).ConfigureAwait(true);
    _permissionCache.TryAdd((sessionId, @object), result);
    return result;
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