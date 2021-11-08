using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Eos.Blazor.Generator.Enum;

namespace Eos.Blazor.Generator.Models
{
  public class VisibleField<T>
  {
    public string Caption { get; set; }
    public Action<T, object> Setter { get; set; }
    public Func<T, object> Getter { get; set; }
    public string Name { get; set; }
    public FieldType Type { get; set; }

    public VisibleField(string name, FieldType type)
    {
      Name = name;
      Caption = name;
      Type = type;
    }
    public VisibleField(string name)
    {
      Name = name;
      Caption = name;
    }

  }
}
