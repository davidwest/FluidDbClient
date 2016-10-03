
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FluidDbClient
{ 
    public static class DataCollectionExtensions
    {
        public static List<T>[] CollectResultSets<T>(this IManagedDbQuery query, int resultCount, Func<IDataRecord, T> map)
        {
            var sets = new List<T>[resultCount];

            var processes = new Action<IEnumerable<IDataRecord>>[resultCount];
            
            for (var i = 0; i != resultCount; i++)
            {
                var index = i;
                processes[i] = data => sets[index] = data.Select(map).ToList();
            }

            query.ProcessResultSets(processes);

            return sets;
        }

        public static List<IDataRecord>[] CollectResultSets(this IManagedDbQuery query, int resultCount)
        {
            return query.CollectResultSets(resultCount, dr => dr.Copy());
        }

        public static List<Dictionary<string, object>>[] CollectResultSetsAsDictionaries(this IManagedDbQuery query, int resultCount)
        {
            return query.CollectResultSets(resultCount, dr => dr.ToDictionary());
        }
    }
}
