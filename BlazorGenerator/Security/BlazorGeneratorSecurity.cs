using BlazorGenerator.Attributes;
using BlazorGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Security
{
  internal class BlazorGeneratorSecurity(IServiceProvider services)
  {
    public ISecurity Security { get; set; } = (ISecurity)services.GetService(typeof(ISecurity))!;

    private Dictionary<Guid, List<PermissionSet>> PermissionCache { get; } = [];

    public PermissionSet GetPermissionSet(Type? Object = null)
    {
      if (PermissionCache.TryGetValue(Security.getCurrentSessionIdentifier(), out var cached))
      {
        if (cached.Any(o => o.Object == Object))
        {
          return cached.First(o => o.Object == Object);
        }
        else if (cached.Any(o => o.Object == null))
        {
          return cached.First(o => o.Object == null);
        }
      }

      var perms = Security.GetPermissionSets(Object);
      PermissionCache.Add(Security.getCurrentSessionIdentifier(), perms);

      return perms.First(o => (o.Object == Object) || (o.Object == null));
    }
  }
}
