using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BlazorGenerator;
using Microsoft.Extensions.DependencyInjection;

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