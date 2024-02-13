using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Models
{
  public class PermissionSet
  {
    public Type? Object { get; set; }
    public bool Insert { get; set; }
    public bool Delete { get; set; }
    public bool Modify { get; set; }
    public bool Execute { get; set; }
    public bool RequireAuthentication { get; set; } = false;
  }
}
