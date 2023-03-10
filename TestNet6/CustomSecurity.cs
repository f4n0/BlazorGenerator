using BlazorGenerator.Security;
using TestNet6.Data;

namespace TestNet6
{
  public class CustomSecurity : ISecurity
  {
    public bool HasPermission(Type ObjToLoad)
    {
      if(ObjToLoad == typeof(TestListPage)) return false;
      return true;
    }
  }
}
