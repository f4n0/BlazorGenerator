using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Blazorise;
using BlazorGenerator.Enum;
using BlazorGenerator.Utils;

namespace BlazorGenerator.Models
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
    public Func<T, string> ToolTip { get; set; }


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

    public static List<VisibleField<T>> loadAllFields()
    {
      var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
      List<VisibleField<T>> list = new List<VisibleField<T>>();
      foreach (var item in propertyInfos)
      {
        list.Add(
            new VisibleField<T>(item.Name)
            {
              Editable = true,
              Getter = f => item.GetValue(f),
              Setter = (f, v) => item.SetValue(f,v),
            }  
        );
      }

      return list;
    }
  }
}
