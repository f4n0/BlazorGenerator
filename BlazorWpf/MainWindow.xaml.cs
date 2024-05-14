using BlazorGenerator;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace BlazorWpf
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      var serviceCollection = new ServiceCollection();
      serviceCollection.AddHttpClient();
      serviceCollection.AddWpfBlazorWebView();
      serviceCollection.AddBlazorGenerator();
      Resources.Add("services", serviceCollection.BuildServiceProvider());
    }
  }
}