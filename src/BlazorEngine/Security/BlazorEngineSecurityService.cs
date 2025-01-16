using BlazorEngine.Models;

namespace BlazorEngine.Security
{
  internal class BlazorEngineSecurityService(IServiceProvider services)
  {
    public ISecurity Security { get; set; } = (ISecurity)services.GetService(typeof(ISecurity))!;

    private Dictionary<string, List<PermissionSet>> PermissionCache { get; } = [];

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
        if (PermissionCache.TryGetValue(sessionId, out var current))
        {
          if (!current.Contains(permissionSet))
          {
            current.Add(permissionSet);
          }
          PermissionCache[sessionId] = current;
        }
        else
        {
          PermissionCache.Add(sessionId, [permissionSet]);
        }
      }

      return permissionSet;
    }

    public async Task<string> GetSessionIdentifier() => await Security.GetCurrentSessionIdentifier().ConfigureAwait(true);
  }
}
