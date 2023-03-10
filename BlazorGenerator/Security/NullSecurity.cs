using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Security
{
  internal class NullSecurity : ISecurity
  {
    public bool HasPermission(Type ObjToLoad)
    {
      return true;
    }
  }
}
