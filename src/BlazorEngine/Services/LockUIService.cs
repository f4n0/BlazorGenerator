namespace BlazorEngine.Services;

public class LockUIService
{
  internal event Action<bool>? OnChange;

  private void NotifyStateChanged(bool val)
  {
    OnChange?.Invoke(val);
  }

  public void LockUI()
  {
    NotifyStateChanged(true);
  }

  public void UnlockUI()
  {
    NotifyStateChanged(false);
  }
}