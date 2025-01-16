using BlazorEngine.Models;

namespace BlazorEngine.Security
{
  internal class NullSecurity : ISecurity
  {
    public Task<string> GetCurrentSessionIdentifier()
    {
      return Task.FromResult(Guid.Empty.ToString());
    }

    public Task<PermissionSet> GetPermissionSet(Type? type = null)
    {
      return Task.FromResult(new PermissionSet()
      {
        Insert = true,
        Delete = true,
        Execute = true,
        Modify = true,
        Object = null!,
        RequireAuthentication = false,
      });
    }
  }
}
