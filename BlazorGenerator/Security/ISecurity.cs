using BlazorGenerator.Models;

namespace BlazorGenerator.Security
{
  public interface ISecurity
  {
    public Task<PermissionSet> GetPermissionSet(Type? type = null);
    public Task<string> getCurrentSessionIdentifier();
  }
}
