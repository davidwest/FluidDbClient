
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


namespace FluidDbClient
{
    public interface IManagedDbQuery
    {
        // --- Synchronous ---

        T GetScalar<T>(T dbNullSubstitue = default(T));
        IDataRecord GetRecord();

        IEnumerable<IDataRecord> GetResultSet();
        
        void ProcessResultSets(params Action<IEnumerable<IDataRecord>>[] processes);
        
        List<IDataRecord>[] CollectResultSets(int resultCount);


        // --- Asynchronous ---

        Task<T> GetScalarAsync<T>(T dbNullSubstitute = default(T));
        Task<IDataRecord> GetRecordAsync();

        Task ProcessResultSetAsync(Action<IDataRecord> process);
        Task<List<IDataRecord>> CollectResultSetAsync();
        Task<List<T>> CollectResultSetAsync<T>(Func<IDataRecord, T> map);

        Task ProcessResultSetsAsync(params Action<IDataRecord>[] processes);
        Task<List<IDataRecord>[]> CollectResultSetsAsync(int resultCount);
    }
}

