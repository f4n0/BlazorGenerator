using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BlazorGenerator.Enum;
using BlazorGenerator.Utils;

namespace BlazorGenerator.Models
{
  public class VisibleField<T>
  {
    public string Caption { get; set; }
    public Action<T, object> Setter { get; set; }
    public Func<T, object> Getter { get; set; }
    public Type fType { get; set; }
    public string Name { get; set; }


  }
}
