using System;
using System.Collections.Generic;
using System.Text;

namespace EHISApi.Common.Extentions
{
    public static class IListExtention
    {
        public static void AddRange<T>(this IList<T> source, IEnumerable<T> newList)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (newList == null)
            {
                throw new ArgumentNullException(nameof(newList));
            }

            if (source is List<T> concreteList)
            {
                concreteList.AddRange(newList);
                return;
            }

            foreach (var element in newList)
            {
                source.Add(element);
            }
        }
    }
}
