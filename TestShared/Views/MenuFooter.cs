using BlazorGenerator.Attributes;
using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestShared.Views
{
  [AddToMenu(Title = "Issue?", Route = "https://github.com/EOS-Solutions/Gordon/issues", Icon = typeof(Icons.Regular.Size16.Archive), OrderSequence = 9999)]
  public class MenuFooter
  {
  }
}
