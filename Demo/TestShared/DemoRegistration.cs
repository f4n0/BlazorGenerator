using BlazorEngine;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Filled;
using TestShared.Views;

namespace TestShared;

public static class DemoRegistration
{
  // All hosts use the same explicit registrations. Modal/edit-only pages stay out of the menu.
  public static void Configure(BlazorEngineOptions options)
  {
    options.AddMenuItem<CardView>("Card Page", CardView.Route, order: 1);
    options.AddMenuItem<CardWithGroups>("Card Page Groups", CardWithGroups.Route, icon: new Size16.ApprovalsApp());
    options.AddMenuItem<ListView>("List Page", ListView.Route, icon: new Size16.AddSquare());
    options.AddMenuItem<SubMenu>("Card Page 1", SubMenu.Route, group: "Sub Menu");
    options.AddMenuItem<SubMenu2>("Card Page 2", SubMenu2.Route, group: "Sub Menu");
    options.AddMenuItem<TwoListView>("TwoList Page", TwoListView.Route);
    options.AddMenuItem<WorksheetView>("Worksheet Page", WorksheetView.Route);
    options.AddFooterItem("Issue?", "https://github.com/f4n0/BlazorGenerator",
      icon: new Size16.Bug(), openInNewTab: true);
  }
}
