using System.Collections.Specialized;
using BlazorGenerator.Attributes;
using BlazorGenerator.Layouts;
using BlazorGenerator.Utils;
using Microsoft.AspNetCore.Components;
using TestShared.Data;

namespace TestShared.Views
{
  [Route("WorksheetView")]
  [Route("WorksheetView/{Param}")]
  [AddToMenu(Title = "Worksheet Page", Route = "WorksheetView")]
  public class WorksheetView : Worksheet<Mock, Mock>
  {
    [Parameter]
    public string? Param { get; set; }

    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public override string Title => "List View";
    public override Type? ListEditFormType => typeof(MockEditForm);

    protected override Task LoadData()
    {
      ListSelected.CollectionChanged += SelectedChanged;
      Content = Mock.GetSingleMock();
      ListContent = Mock.GetMultipleMock(16);
      return Task.CompletedTask;
    }

    private void SelectedChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.NewItems != null)
      {
        Content = (Mock)e.NewItems[e.NewStartingIndex];
        InvokeAsync(StateHasChanged);
      }
    }

    protected override Task LoadVisibleFields()
    {
      ShowButtons = false;

      VisibleFields = [];
      VisibleFields.AddField(nameof(Mock.Id));
      VisibleFields.AddField(nameof(Mock.Name));
      VisibleFields.AddField(nameof(Mock.Price));
      VisibleFields.AddField(nameof(Mock.Description));
      VisibleFields.AddField(nameof(Mock.OrderDate));

      ListVisibleFields = [];
      ListVisibleFields.AddField(nameof(Mock.Id));
      ListVisibleFields.AddField(nameof(Mock.Name)).AddFieldProperty(prop => prop.TextStyle = (_) => BlazorGenerator.Enum.TextStyle.Italic);
      ListVisibleFields.AddField(nameof(Mock.Description));
      return Task.CompletedTask;
    }

    [PageAction(Caption="Test")]
    public void Test()
    {
      var test = ListSelected;
    }
  }
}
