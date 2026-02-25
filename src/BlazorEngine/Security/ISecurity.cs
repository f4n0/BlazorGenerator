using BlazorEngine.Models;

namespace BlazorEngine.Security;

public interface ISecurity
{
  public Task<PermissionSet> GetPermissionSet(Type? type = null);
  public Task<string> GetCurrentSessionIdentifier();
}