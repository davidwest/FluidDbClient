using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FluidDbClient
{
    public static class DataCollectionAsyncExtensions
    {
        public static async Task<List<T>> CollectResultSetAsync<T>(this IManagedDbQuery query, Func<IDataRecord, T> map)
        {
            var list = new List<T>();
            await query.ProcessResultSetAsync(rec => list.Add(map(rec)));

            return list;
        }

        public static async Task<List<IDataRecord>> CollectResultSetAsync(this IManagedDbQuery query)
        {
            return await query.CollectResultSetAsync(rec => rec.Copy());
        }

        public static async Task<List<Dictionary<string, object>>> CollectResultSetAsDictionariesAsync(this IManagedDbQuery query)
        {
            return await query.CollectResultSetAsync(dr => dr.ToDictionary());
        }
        
        
        public static async Task<List<T>[]> CollectResultSetsAsync<T>(this IManagedDbQuery query, int resultCount, Func<IDataRecord, T> map)
        {
            var sets = new List<T>[resultCount];

            var processes = new Action<IDataRecord>[resultCount];

            for (var i = 0; i != resultCount; i++)
            {
                var index = i;
                sets[index] = new List<T>();
                processes[i] = data => sets[index].Add(map(data));
            }

            await query.ProcessResultSetsAsync(processes);

            return sets;
        }

        public static async Task<List<IDataRecord>[]> CollectResultSetsAsync(this IManagedDbQuery query, int resultCount)
        {
            return await query.CollectResultSetsAsync(resultCount, rec => rec.Copy());
        }

        public static async Task<List<Dictionary<string, object>>[]> CollectResultSetsAsDictionariesAsync(this IManagedDbQuery query, int resultCount)
        {
            return await query.CollectResultSetsAsync(resultCount, rec => rec.ToDictionary());
        }
    }
}
