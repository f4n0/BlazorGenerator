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
    public Guid getCurrentSessionIdentifier()
    {
      return Guid.Empty;
    }


    public List<PermissionSet> GetPermissionSets(Type type = null)
    {
      return new List<PermissionSet>(){new  PermissionSet()
      {
        Insert = true,
        Delete = true,
        Execute = true,
        Modify = true,
        Object = null
      } };
    }

  }
}
