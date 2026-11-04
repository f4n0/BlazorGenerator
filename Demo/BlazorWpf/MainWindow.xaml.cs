using BlazorEngine;
using Microsoft.Extensions.DependencyInjection;
using TestShared;

namespace BlazorWpf;

/// <summary>
///   Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
  public MainWindow()
  {
    InitializeComponent();
    var serviceCollection = new ServiceCollection();
    serviceCollection.AddHttpClient();
    serviceCollection.AddWpfBlazorWebView();
    serviceCollection.AddBlazorEngine(DemoRegistration.Configure);
    serviceCollection.AddBlazorWebViewDeveloperTools();
    Resources.Add("services", serviceCollection.BuildServiceProvider());
  }
}
