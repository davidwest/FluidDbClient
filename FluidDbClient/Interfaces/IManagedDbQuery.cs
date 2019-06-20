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

        DataTable GetDataTable(string tableName = null);

        DataSet GetDataSet(params string[] tableNames);

        void ProcessResultSets(params Action<IEnumerable<IDataRecord>>[] processes);
        

        // --- Asynchronous ---

        Task<T> GetScalarAsync<T>(T dbNullSubstitute = default(T));

        Task<IDataRecord> GetRecordAsync();

        Task<DataTable> GetDataTableAsync(string tableName = null);
        
        Task<DataSet> GetDataSetAsync(params string[] tableNames);

        Task ProcessResultSetAsync(Action<IDataRecord> process);

        Task ProcessResultSetsAsync(params Action<IDataRecord>[] processes);
    }
}

