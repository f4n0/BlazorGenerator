using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator
{
  public class BlazorGeneratorSettings
  {
    private static BlazorGeneratorSettings _instance;
    public static BlazorGeneratorSettings Instance => _instance ?? (_instance = new BlazorGeneratorSettings());

    public string ApplicationName { get; set; } = "BlazorGenerator App";
    public OfficeColor BaseColor { get; set; } = OfficeColor.Default;

    public string UnauthorizedRoute { get; set; } = "/Unauthorized";
    public string LoginRoute { get; set; } = "/Login";

  }
}
