using System;
using System.Collections.Generic;
using System.Linq;

namespace FluidDbClient
{
    public static class NestedModelMappingExtensions
    {
        public static IEnumerable<T1> MapNested<TKey1, TKey2, T1, T2, TSource>
              (this IEnumerable<TSource> source,
               Func<TSource, TKey1> selectKey1,
               Func<TSource, TKey2> selectKey2,
               Func<TSource, IEnumerable<T2>, T1> mapLevel1,
               Func<TSource, T2> mapLevel2)
        {
            var lookup1 = source.ToLookup(selectKey1);

            var result = 
                from grp1 in lookup1
                let root1 = grp1.First()
                where !selectKey1(root1).Equals(default(TKey1))
                select mapLevel1(root1, from sourceItem in grp1
                                        where !selectKey2(sourceItem).Equals(default(TKey2))
                                        select mapLevel2(sourceItem));

            return result;
        } 


        public static IEnumerable<T1> MapNested<TKey1, TKey2, TKey3, T1, T2, T3, TSource>
              (this IEnumerable<TSource> source,
               Func<TSource, TKey1> selectKey1,
               Func<TSource, TKey2> selectKey2,
               Func<TSource, TKey3> selectKey3,
               Func<TSource, IEnumerable<T2>, T1> mapLevel1,
               Func<TSource, IEnumerable<T3>, T2> mapLevel2,
               Func<TSource, T3> mapLevel3)
        {
            var lookup1 = source.ToLookup(selectKey1);
            
            var result = 
                from grp1 in lookup1
                let root1 = grp1.First()
                where !selectKey1(root1).Equals(default(TKey1))
                let lookup2 = grp1.ToLookup(selectKey2)
                select mapLevel1(root1, from grp2 in lookup2
                                        let root2 = grp2.First()
                                        where !selectKey2(root2).Equals(default(TKey2))
                                        select mapLevel2(root2, from sourceItem in grp2
                                                                where !selectKey3(sourceItem).Equals(default(TKey3))
                                                                select mapLevel3(sourceItem)));

            return result;
        }

        
        public static IEnumerable<T1> MapNested<TKey1, TKey2, TKey3, TKey4, T1, T2, T3, T4, TSource>
              (this IEnumerable<TSource> source,
               Func<TSource, TKey1> selectKey1,
               Func<TSource, TKey2> selectKey2,
               Func<TSource, TKey3> selectKey3,
               Func<TSource, TKey4> selectKey4,
               Func<TSource, IEnumerable<T2>, T1> mapLevel1,
               Func<TSource, IEnumerable<T3>, T2> mapLevel2,
               Func<TSource, IEnumerable<T4>, T3> mapLevel3,
               Func<TSource, T4> mapLevel4)
        {
            var lookup1 = source.ToLookup(selectKey1);

            var result =
                from grp1 in lookup1
                let root1 = grp1.First()
                where !selectKey1(root1).Equals(default(TKey1))
                let lookup2 = grp1.ToLookup(selectKey2)
                select mapLevel1(root1, from grp2 in lookup2
                                        let root2 = grp2.First()
                                        where !selectKey2(root2).Equals(default(TKey2))
                                        let lookup3 = grp2.ToLookup(selectKey3)
                                        select mapLevel2(root2, from grp3 in lookup3
                                                                let root3 = grp3.First()
                                                                where !selectKey3(root3).Equals(default(TKey3))
                                                                select mapLevel3(root3, from sourceItem in grp3
                                                                                        where !selectKey4(sourceItem).Equals(default(TKey4))
                                                                                        select mapLevel4(sourceItem))));

            return result;
        }
    }
}
