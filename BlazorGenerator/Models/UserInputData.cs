﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Models
{
  public class UserInputData
  {
    public string Message { get; set; }
    public string Result { get; set; }
    public UserInputType InputType { get; set; } = UserInputType.PlainText;
    public List<string> Choices { get; set; }
  }

  public enum UserInputType
  {
    PlainText,
    Secret,
    Choice
  }
}
