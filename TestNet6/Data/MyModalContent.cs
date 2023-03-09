using BlazorGenerator.Dialogs;
using BlazorGenerator.Models;

namespace TestNet6.Data
{
  public class MyModalContent : ModalPage<MyModalContent>
  {
    public override string Title => "Is this a modal? Oh Yeah!";
    public override bool ShowSave => true;
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    public string? Summary { get; set; }
    public string? Summary2 { get; set; }
    public string? Summary3 { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.


    protected override void OnInitialized()
    {
      VisibleFields = new List<VisibleField<MyModalContent>>() {
        new VisibleField<MyModalContent>(nameof(Summary)){Getter = f => f.Summary, Setter = (f,v)=>f.Summary = v.ToString(), Editable = true },
        new VisibleField<MyModalContent>(nameof(Summary2)){Getter = f => f.Summary2, Setter = (f,v)=>f.Summary2 = v.ToString(), Editable = true},
        new VisibleField<MyModalContent>(nameof(Summary3)){Getter = f => f.Summary3, Setter = (f,v)=>f.Summary3 = v.ToString(), Editable = true}
      };
    }  

  
  }
}
