using BlazorGenerator.Attributes;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Security
{
  internal class BlazorGenSecurity
  {
    public BlazorGenSecurity(IServiceProvider services)
    {
      Security = services.GetService(typeof(ISecurity)) as ISecurity;
    }


    public ISecurity Security { get; set; }

    private Dictionary<Guid, List<PermissionSet>> PermissionCache { get; set; } = new();


    public PermissionSet GetPermissionSet(Type Object = null)
    {
      if (PermissionCache.TryGetValue(Security.getCurrentSessionIdentifier(), out var cached))
        if (cached.Any(o => (o.Object == Object)))
        {
          return cached.Where(o => (o.Object == Object)).First();
        }
        else if (cached.Any(o => (o.Object == null)))
        {
          return cached.Where(o => (o.Object == null)).First();

        }

      var perms = Security.GetPermissionSets(Object);
      PermissionCache.Add(Security.getCurrentSessionIdentifier(), perms);

      return perms.Where(o => (o.Object == Object)||(o.Object == null)).First();
    }

  }
}
