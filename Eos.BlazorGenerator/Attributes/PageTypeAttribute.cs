using Microsoft.AspNetCore.Components;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Eos.BlazorGenerator.Enum;

namespace Eos.BlazorGenerator.Attributes
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class PageTypeAttribute : Attribute
  {
    public PageTypeAttribute(PageTypes type)
    {
      Type = type;
    }

    public PageTypes Type { get; private set; }
  }

}
