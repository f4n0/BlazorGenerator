using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorGenerator.Models;

namespace BlazorGenerator.Security
{
  public interface ISecurity
  {
    public Task<PermissionSet> GetPermissionSet(Type? type = null);
    public Task<string> getCurrentSessionIdentifier();
  }
}
