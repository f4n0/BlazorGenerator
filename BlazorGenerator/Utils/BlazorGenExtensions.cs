using BlazorGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGenerator.Utils
{
    public static class BlazorGenExtensions
    {
        public static void AddField<T>(this List<VisibleField<T>> visibleFields, string propertyName, Func<VisibleField<T>, VisibleField<T>> additionalProperties = null) where T : class
        {
            var prop = typeof(T).GetProperty(propertyName);


            var field = new VisibleField<T>()
            {
                Name = propertyName,
                fType = prop.PropertyType,
                Caption = propertyName,
                Getter = f => prop.GetValue(f),
                Setter = (f, v) => prop.SetValue(f, v)
            };

            if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                field.fType = Nullable.GetUnderlyingType(prop.PropertyType);
            }



            if (additionalProperties != null)
                field = additionalProperties(field);

            visibleFields.Add(field);
        }
    }
}
