using System.Reflection;
using BlazorEngine.Components.Field;
using BlazorEngine.Models;

namespace BlazorEngine.Tests;

public class FormFieldTests
{
  [Fact]
  public void LookupSetterUsesTheValueStoredByTheModel()
  {
    var data = new LookupModel();
    var field = new VisibleField<LookupModel>
    {
      FieldType = typeof(string),
      Get = args => args.Data.FeedName,
      Set = args =>
      {
        var selected = string.IsNullOrWhiteSpace(args.Data.FeedName)
          ? []
          : args.Data.FeedName.Split(';').ToList();
        var value = args.Value!.ToString()!;

        if (!selected.Remove(value))
          selected.Add(value);

        args.Data.FeedName = string.Join(';', selected);
      }
    };
    #pragma warning disable BL0005 // Component parameters are set directly for this unit test.
    var formField = new FormField<LookupModel>
    {
      Field = field,
      Data = data
    };
    #pragma warning restore BL0005

    var setFieldValue = typeof(FormField<LookupModel>)
      .GetMethod("SetFieldValue", BindingFlags.Instance | BindingFlags.NonPublic)!;
    var currentValue = typeof(FormField<LookupModel>)
      .GetField("_currentValue", BindingFlags.Instance | BindingFlags.NonPublic)!;

    setFieldValue.Invoke(formField, ["first"]);
    setFieldValue.Invoke(formField, ["second"]);

    Assert.Equal("first;second", data.FeedName);
    Assert.Equal("first;second", currentValue.GetValue(formField));
  }

  private sealed class LookupModel
  {
    public string FeedName { get; set; } = string.Empty;
  }
}
