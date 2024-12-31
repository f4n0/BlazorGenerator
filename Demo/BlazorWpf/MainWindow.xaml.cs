using BlazorGenerator;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorWpf
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    public MainWindow()
    {
      InitializeComponent();
      var serviceCollection = new ServiceCollection();
      serviceCollection.AddHttpClient();
      serviceCollection.AddWpfBlazorWebView();
      serviceCollection.AddBlazorGenerator();
      serviceCollection.AddBlazorWebViewDeveloperTools();
      Resources.Add("services", serviceCollection.BuildServiceProvider());
    }
  }
}