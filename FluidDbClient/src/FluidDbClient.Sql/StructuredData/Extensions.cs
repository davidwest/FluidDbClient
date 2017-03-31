using System;
using System.Collections.Generic;

namespace FluidDbClient.Sql
{
    public static class Extensions
    {
        public static StructuredData ToStructuredData<T>(this IEnumerable<T> items, string tableTypeName, Func<T, object> map = null)
        {
            map = map ?? (item => item);

            var builder = new StructuredDataBuilder(tableTypeName);

            foreach (var item in items)
            {
                builder.Append(map(item));
            }

            return builder.Build();
        }
    }
}
