namespace BlazorGenerator.Services
{
  public class ProgressService
  {
    internal event Action<bool>? OnChange;

    private void NotifyStateChanged(bool val) => OnChange?.Invoke(val);

    public void StartProgress()
    {
      NotifyStateChanged(true);
    }

    public void StopProgress()
    {
      NotifyStateChanged(false);
    }
  }
}
