using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eos.Blazor.Generator.Attributes
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class BasicActions : Attribute
  {
    public BasicActions(bool editAction, bool newAction, bool deleteAction)
    {
      EditAction = editAction;
      NewAction = newAction;
      DeleteAction = deleteAction;
    }
    public BasicActions(bool editAction, bool newAction)
    {
      EditAction = editAction;
      NewAction = newAction;
    }
    public BasicActions(bool editAction)
    {
      EditAction = editAction;
    }

    public bool EditAction { get; private set; }
    public bool NewAction { get; private set; }
    public bool DeleteAction { get; private set; }


  }
}
