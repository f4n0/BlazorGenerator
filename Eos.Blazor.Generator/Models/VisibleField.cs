using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Blazorise;
using Eos.Blazor.Generator.Enum;

namespace Eos.Blazor.Generator.Models
{
  public class VisibleField<T>
  {
    public string Caption { get; set; }
    public Action<T, object> Setter { get; set; }
    public Func<T, object> Getter { get; set; }
    public string Name { get; set; }
    public FieldType FieldType { get; set; }
    public bool Editable { get; set; } = false;
    public bool EditOnly { get; set; } = false;
    public string[] Values { get; set; }
    public TextRole TextRole { get; set; } = TextRole.Text;


    public VisibleField(string name, FieldType type, bool editable)
    {
      Name = name;
      Caption = name;
      FieldType = type;
      Editable = editable;
    }
    public VisibleField(string name, FieldType type)
    {
      Name = name;
      Caption = name;
      FieldType = type;
    }
    public VisibleField(string name)
    {
      Name = name;
      Caption = name;
      FieldType = FieldType.Text;
    }
  }
}
