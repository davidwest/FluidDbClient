using System;
using System.Collections.Generic;

namespace FluidDbClient.Extensions
{
    internal static class InternalEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> performAction)
        {
            foreach (var item in sequence)
            {
                performAction(item);
            }
        }
    }
}
