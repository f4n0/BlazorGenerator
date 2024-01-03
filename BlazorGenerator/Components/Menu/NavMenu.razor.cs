using BlazorGenerator.Attributes;
using BlazorGenerator.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Components.Menu
{
    public partial class NavMenu
    {
        Dictionary<string, int> MenuGroups { get; set; }

        List<AddToMenuAttribute> menus { get; set; } = new List<AddToMenuAttribute>();

        void PopulateDictionary()
        {
            menus.Clear();
            var allMenu = Utils.AttributesUtils.GetModelsWithAttribute<AddToMenuAttribute>();
            MenuGroups = new Dictionary<string, int>();
            foreach (var item in allMenu)
            {
                if (Security.GetPermissionSet(item.Type).Execute)
                {
                    if (!menus.Contains(item.Attribute))
                        menus.Add(item.Attribute);
                    if (MenuGroups.ContainsKey(item.Attribute.Group))
                    {
                        MenuGroups[item.Attribute.Group]++;
                    }
                    else
                    {
                        MenuGroups.Add(item.Attribute.Group, 1);
                    }
                }
            }
        }
    }
}
