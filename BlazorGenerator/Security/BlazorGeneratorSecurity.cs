using BlazorGenerator.Models;

namespace BlazorGenerator.Security
{
  internal class BlazorGeneratorSecurity(IServiceProvider services)
  {
    public ISecurity Security { get; set; } = (ISecurity)services.GetService(typeof(ISecurity))!;

    private Dictionary<string, List<PermissionSet>> PermissionCache { get; } = [];

    public async Task<PermissionSet> GetPermissionSet(Type? Object = null)
    {
      PermissionSet permissionSet = null;
      var sessionId = await Security.getCurrentSessionIdentifier();
      if (string.IsNullOrEmpty(sessionId))
      {
        return await Security.GetPermissionSet(Object);
      }
      if (PermissionCache.TryGetValue(sessionId, out var cached))
      {
        if (cached.Any(o => o.Object == Object))
        {
          permissionSet = cached.First(o => o.Object == Object);
        }
        else if (cached.Any(o => o.Object == null))
        {
          permissionSet = cached.First(o => o.Object == null);
        }
      }

      if (permissionSet == null)
      {
        permissionSet = await Security.GetPermissionSet(Object);
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


    public async Task<string> GetSessionIdentifier() => await Security.getCurrentSessionIdentifier();
  }
}
