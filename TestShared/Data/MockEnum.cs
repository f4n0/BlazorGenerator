using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestShared.Data
{
  public enum MockEnum
  {

    [Display(Name = "null")]
    None,
    Item,
    Account
  }
}
