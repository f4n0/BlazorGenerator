﻿using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorGenerator.Attributes
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class FooterLinkAttribute : Attribute
  {
    public required string Title { get; set; }
    public required string Route { get; set; }
    public Type Icon { get; set; } = typeof(Icons.Regular.Size20.Balloon);
  }
}
