using System.Collections.Concurrent;
using BlazorEngine.Models;

namespace BlazorEngine.Security
{
  internal class BlazorEngineSecurityService(IServiceProvider services)
  {
    public ISecurity Security { get; set; } = (ISecurity)services.GetService(typeof(ISecurity))!;

    private ConcurrentDictionary<string, ConcurrentBag<PermissionSet>> PermissionCache { get; } = new();

    public async Task<PermissionSet> GetPermissionSet(Type? @object = null)
    {
      PermissionSet? permissionSet = null;
      var sessionId = await Security.GetCurrentSessionIdentifier().ConfigureAwait(true);
      if (string.IsNullOrEmpty(sessionId))
      {
        return await Security.GetPermissionSet(@object).ConfigureAwait(true);
      }
      if (PermissionCache.TryGetValue(sessionId, out var cached))
      {
        if (cached.Any(o => o.Object == @object))
        {
          permissionSet = cached.First(o => o.Object == @object);
        }
        else if (cached.Any(o => o.Object == null))
        {
          permissionSet = cached.First(o => o.Object == null);
        }
      }

      if (permissionSet == null)
      {
        permissionSet = await Security.GetPermissionSet(@object).ConfigureAwait(true);
        var bag = PermissionCache.GetOrAdd(sessionId, _ => new ConcurrentBag<PermissionSet>());
        if (!bag.Contains(permissionSet))
        {
          bag.Add(permissionSet);
        }
      }

      return permissionSet;
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

    public async Task<string> GetSessionIdentifier() => await Security.GetCurrentSessionIdentifier().ConfigureAwait(true);
  }
}
