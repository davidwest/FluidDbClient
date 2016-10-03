
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
        
        // --- Asynchronous ---

        Task<T> GetScalarAsync<T>(T dbNullSubstitute = default(T));
        Task<IDataRecord> GetRecordAsync();
        Task ProcessResultSetAsync(Action<IDataRecord> process);
        Task ProcessResultSetsAsync(params Action<IDataRecord>[] processes);
    }
}

