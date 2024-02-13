using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorGenerator.Models;

namespace BlazorGenerator.Security
{
  internal class NullSecurity : ISecurity
  {
    public Task<string> getCurrentSessionIdentifier()
    {
      return Task.FromResult(Guid.Empty.ToString());
    }

    public Task<PermissionSet> GetPermissionSet(Type? type = null)
    {
      return Task.FromResult<PermissionSet>(new PermissionSet()
      {
        Insert = true,
        Delete = true,
        Execute = true,
        Modify = true,
        Object = null!,
        RequireAuthentication = false,
      });
    }
  }
}
