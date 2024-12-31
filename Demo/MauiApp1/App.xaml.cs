namespace MauiApp1
{
  public partial class App
  {
    public App()
    {
      InitializeComponent();
    }
    protected override Window CreateWindow(IActivationState? activationState)
    {
      return new Window(new MainPage());
    }
  }
}
