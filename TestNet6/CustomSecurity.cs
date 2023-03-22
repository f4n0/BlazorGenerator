using BlazorGenerator.Security;
using TestNet6.Data;

namespace TestNet6
{
  public class CustomSecurity : ISecurity
  {
    public Guid getCurrentSessionIdentifier()
    {
      throw new NotImplementedException();
    }


    public List<PermissionSet> GetPermissionSets(Type type = null)
    {
      throw new NotImplementedException();
    }

  }
}
