using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Eos.Blazor.Generator.Models
{
  public class VisibleField<T>
  {
    public string Caption { get; set; }
    public Action<T, object> Setter { get; set; }
    public Func<T, string> Getter { get; set; }
    public string Name { get; set; }
  }
}
