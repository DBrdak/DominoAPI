using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UtilityLibrary
{
    public static class Methods
    {
        public static void MapTo(this object source, object destination)
        {
            foreach (var sourceProp in source.GetType().GetProperties())
            {
                foreach (var destinationProp in destination.GetType().GetProperties())
                {
                    var dtoPropValue = sourceProp.GetValue(source);
                    if (dtoPropValue != null && sourceProp.Name == destinationProp.Name)
                    {
                        destinationProp.SetValue(destination, dtoPropValue);
                    }
                }
            }
        }

        public static object GetProperty<T>(this T obj, string propName)
        {
            return obj.GetType().GetProperty(propName).GetValue(obj, null);
        }

        public static IEnumerable<T> Sort<T>(this IEnumerable<T> list, string sortBy, string sortDirection)
        {
            if (!string.IsNullOrEmpty(sortBy))
            {
                return sortDirection == "ASC"
                    ? list.OrderBy(p => p.GetProperty(sortBy))
                    : list.OrderByDescending(p => p.GetProperty(sortBy));
            }

            return list;
        }

        public static IEnumerable<T> GetPage<T>(this IEnumerable<T> baseList, int pageSize, int pageId)
        {
            return baseList
                .Skip(pageSize * (pageId - 1))
                .Take(pageSize);
        }
    }
}