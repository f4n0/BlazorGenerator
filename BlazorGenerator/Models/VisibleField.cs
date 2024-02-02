using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BlazorGenerator.Enum;
using BlazorGenerator.Utils;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator.Models
{
  public class VisibleField<T>
  {
    public string Caption { get; set; } = null!;
    public required Action<T, object> Setter { get; set; }
    public required Func<T, object?> Getter { get; set; }
    public required Type FieldType { get; set; }
    public required string Name { get; set; }
    public TextFieldType TextFieldType { get; set; } = TextFieldType.Text;
  }
}
