﻿using System.Collections.Specialized;
using Microsoft.AspNetCore.Components;
using TestShared.Data;
using BlazorEngine.Attributes;
using BlazorEngine.Layouts;
using BlazorEngine.Utils;

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

    public override bool ShowExportToExcel => false;
    public override string Title => "List View";
    public override Type? ListEditFormType => typeof(MockEditForm);

    protected override Task LoadData()
    {
      ListSelected.CollectionChanged += SelectedChanged;
      Content = Mock.GetSingleMock();
      ListContent = Mock.GetMultipleMock(30);
      return Task.CompletedTask;
    }

    private void SelectedChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.NewItems != null)
      {
        Content = (Mock)e.NewItems[0];
        RefreshCard();
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
      ListVisibleFields.AddField(nameof(Mock.Name)).AddFieldProperty(prop => prop.TextStyle = (_) => BlazorEngine.Enum.TextStyle.Italic);
      ListVisibleFields.AddField(nameof(Mock.Description));
      return Task.CompletedTask;
    }

    [PageAction(Caption="Test")]
    public void Test()
    {
      ListContent = Mock.GetMultipleMock(2);
      StateHasChanged();
    }
  }
}
