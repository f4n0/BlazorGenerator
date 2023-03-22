using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Security
{
  public interface ISecurity
  {
    public List<PermissionSet> GetPermissionSets(Type type = null);
    public Guid getCurrentSessionIdentifier();
  }
}
