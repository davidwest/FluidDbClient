using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FluidDbClient.Shell
{
    public static class DbProc
    {
        // TODO: add all versions that take DbConnection or DbTransaction

        #region --- Synchronous Queries ---

        public static T GetScalar<T>(string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery(procedureName, parameters).GetScalar<T>();
        }

        public static T GetScalar<T>(DbSessionBase session, string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery(session, procedureName, parameters).GetScalar<T>();
        }

        public static T GetScalar<T>(string procedureName, T dbNullSubstitute, object parameters = null)
        {
            return new StoredProcedureDbQuery(procedureName, parameters).GetScalar(dbNullSubstitute);
        }

        public static T GetScalar<T>(DbSessionBase session, string procedureName, T dbNullSubstitute, object parameters = null)
        {
            return new StoredProcedureDbQuery(session, procedureName, parameters).GetScalar(dbNullSubstitute);
        }



        public static IDataRecord GetRecord(string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery(procedureName, parameters).GetRecord();
        }

        public static IDataRecord GetRecord(DbSessionBase session, string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery(session, procedureName, parameters).GetRecord();
        }



        public static IEnumerable<IDataRecord> GetResultSet(string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery(procedureName, parameters).GetResultSet();
        }

        public static IEnumerable<IDataRecord> GetResultSet(DbSessionBase session, string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery(session, procedureName, parameters).GetResultSet();
        }



        public static DataTable GetDataTable(string procedureName, string tableName, object parameters = null)
        {
            return new StoredProcedureDbQuery(procedureName, parameters).GetDataTable(tableName);
        }

        public static DataTable GetDataTable(DbSessionBase session, string procedureName, string tableName, object parameters = null)
        {
            return new StoredProcedureDbQuery(session, procedureName, parameters).GetDataTable(tableName);
        }


        public static DataSet GetDataSet(string procedureName, string[] tableNames, object parameters = null)
        {
            return new StoredProcedureDbQuery(procedureName, parameters).GetDataSet(tableNames);
        }

        public static DataSet GetDataSet(DbSessionBase session, string procedureName, string[] tableNames, object parameters = null)
        {
            return new StoredProcedureDbQuery(session, procedureName, parameters).GetDataSet(tableNames);
        }
        


        public static void ProcessResultSets(string procedureName, object parameters, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new StoredProcedureDbQuery(procedureName, parameters).ProcessResultSets(processes);
        }

        public static void ProcessResultSets(DbSessionBase session, string procedureName, object parameters, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new StoredProcedureDbQuery(session, procedureName, parameters).ProcessResultSets(processes);
        }

        public static void ProcessResultSets(string procedureName, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new StoredProcedureDbQuery(procedureName).ProcessResultSets(processes);
        }

        public static void ProcessResultSets(DbSessionBase session, string procedureName, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new StoredProcedureDbQuery(session, procedureName).ProcessResultSets(processes);
        }


        public static List<T>[] CollectResultSets<T>(int resultCount, string procedureName, Func<IDataRecord, T> map, object parameters = null)
        {
            return new StoredProcedureDbQuery(procedureName, parameters).CollectResultSets(resultCount, map);
        }

        public static List<T>[] CollectResultSets<T>(DbSessionBase session, int resultCount, string procedureName, Func<IDataRecord, T> map, object parameters = null)
        {
            return new StoredProcedureDbQuery(procedureName, parameters).CollectResultSets(resultCount, map);
        }


        public static List<IDataRecord>[] CollectResultSets(int resultCount, string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery(procedureName, parameters).CollectResultSets(resultCount);
        }

        public static List<IDataRecord>[] CollectResultSets(DbSessionBase session, int resultCount, string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery(procedureName, parameters).CollectResultSets(resultCount);
        }


        public static List<Dictionary<string, object>>[] CollectResultSetsAsDictionaries(int resultCount, string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery(procedureName, parameters).CollectResultSetsAsDictionaries(resultCount);
        }

        public static List<Dictionary<string, object>>[] CollectResultSetsAsDictionaries(DbSessionBase session, int resultCount, string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery(procedureName, parameters).CollectResultSetsAsDictionaries(resultCount);
        }

        #endregion


        #region --- Async Queries ---

        public static async Task<T> GetScalarAsync<T>(string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery(procedureName, parameters).GetScalarAsync<T>();
        }

        public static async Task<T> GetScalarAsync<T>(DbSessionBase session, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery(session, procedureName, parameters).GetScalarAsync<T>();
        }

        public static async Task<T> GetScalarAsync<T>(string procedureName, T dbNullSubstitute, object parameters = null)
        {
            return await new StoredProcedureDbQuery(procedureName, parameters).GetScalarAsync(dbNullSubstitute);
        }

        public static async Task<T> GetScalarAsync<T>(DbSessionBase session, T dbNullSubstitute, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery(session, procedureName, parameters).GetScalarAsync(dbNullSubstitute);
        }



        public static async Task<IDataRecord> GetRecordAsync(string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery(procedureName, parameters).GetRecordAsync();
        }

        public static async Task<IDataRecord> GetRecordAsync(DbSessionBase session, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery(session, procedureName, parameters).GetRecordAsync();
        }



        public static async Task<DataTable> GetDataTableAsync(string procedureName, string tableName, object parameters = null)
        {
            return await new StoredProcedureDbQuery(procedureName, parameters).GetDataTableAsync(tableName);
        }

        public static async Task<DataTable> GetDataTableAsync(DbSessionBase session, string procedureName, string tableName, object parameters = null)
        {
            return await new StoredProcedureDbQuery(session, procedureName, parameters).GetDataTableAsync(tableName);
        }


        public static async Task<DataSet> GetDataSetAsync(string procedureName, string[] tableNames, object parameters = null)
        {
            return await new StoredProcedureDbQuery(procedureName, parameters).GetDataSetAsync(tableNames);
        }

        public static async Task<DataSet> GetDataSetAsync(DbSessionBase session, string procedureName, string[] tableNames, object parameters = null)
        {
            return await new StoredProcedureDbQuery(session, procedureName, parameters).GetDataSetAsync(tableNames);
        }



        public static async Task ProcessResultSetAsync(string procedureName, Action<IDataRecord> process, object parameters = null)
        {
            await new StoredProcedureDbQuery(procedureName, parameters).ProcessResultSetAsync(process);
        }

        public static async Task ProcessResultSetAsync(DbSessionBase session, string procedureName, Action<IDataRecord> process, object parameters = null)
        {
            await new StoredProcedureDbQuery(session, procedureName, parameters).ProcessResultSetAsync(process);
        }


        public static async Task<List<T>> CollectResultSetAsync<T>(string procedureName, Func<IDataRecord, T> map, object parameters = null)
        {
            return await new StoredProcedureDbQuery(procedureName, parameters).CollectResultSetAsync(map);
        }

        public static async Task<List<T>> CollectResultSetAsync<T>(DbSessionBase session, string procedureName, Func<IDataRecord, T> map, object parameters = null)
        {
            return await new StoredProcedureDbQuery(session, procedureName, parameters).CollectResultSetAsync(map);
        }


        public static async Task<List<IDataRecord>> CollectResultSetAsync(string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery(procedureName, parameters).CollectResultSetAsync();
        }

        public static async Task<List<IDataRecord>> CollectResultSetAsync(DbSessionBase session, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery(session, procedureName, parameters).CollectResultSetAsync();
        }


        public static async Task<List<Dictionary<string, object>>> CollectResultSetAsDictionariesAsync(string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery(procedureName, parameters).CollectResultSetAsDictionariesAsync();
        }

        public static async Task<List<Dictionary<string, object>>> CollectResultSetAsDictionariesAsync(DbSessionBase session, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery(session, procedureName, parameters).CollectResultSetAsDictionariesAsync();
        }



        public static async Task ProcessResultSetsAsync(string procedureName, params Action<IDataRecord>[] processes)
        {
            await new StoredProcedureDbQuery(procedureName).ProcessResultSetsAsync(processes);
        }

        public static async Task ProcessResultSetsAsync(DbSessionBase session, string procedureName, params Action<IDataRecord>[] processes)
        {
            await new StoredProcedureDbQuery(session, procedureName).ProcessResultSetsAsync(processes);
        }

        public static async Task ProcessResultSetsAsync(string procedureName, object parameters, params Action<IDataRecord>[] processes)
        {
            await new StoredProcedureDbQuery(procedureName, parameters).ProcessResultSetsAsync(processes);
        }

        public static async Task ProcessResultSetsAsync(DbSessionBase session, string procedureName, object parameters, params Action<IDataRecord>[] processes)
        {
            await new StoredProcedureDbQuery(session, procedureName, parameters).ProcessResultSetsAsync(processes);
        }


        public static async Task<List<T>[]> CollectResultSetsAsync<T>(int resultCount, Func<IDataRecord, T> map, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery(procedureName, parameters).CollectResultSetsAsync(resultCount, map);
        }

        public static async Task<List<T>[]> CollectResultSetsAsync<T>(DbSessionBase session, int resultCount, Func<IDataRecord, T> map, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery(session, procedureName, parameters).CollectResultSetsAsync(resultCount, map);
        }


        public static async Task<List<IDataRecord>[]> CollectResultSetsAsync(int resultCount, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery(procedureName, parameters).CollectResultSetsAsync(resultCount);
        }
        
        public static async Task<List<IDataRecord>[]> CollectResultSetsAsync(DbSessionBase session, int resultCount, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery(session, procedureName, parameters).CollectResultSetsAsync(resultCount);
        }


        public static async Task<List<Dictionary<string, object>>[]> CollectResultSetsAsDictionariesAsync(int resultCount, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery(procedureName, parameters).CollectResultSetsAsDictionariesAsync(resultCount);
        }
        
        public static async Task<List<Dictionary<string, object>>[]> CollectResultSetsAsDictionariesAsync(DbSessionBase session, int resultCount, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery(session, procedureName, parameters).CollectResultSetsAsDictionariesAsync(resultCount);
        }

        #endregion


        #region --- Synchronous Commands ---

        public static void Execute(string procedureName, object parameters = null)
        {
            new StoredProcedureDbCommand(procedureName, parameters).Execute();
        }

        public static void Execute(DbSessionBase session, string procedureName, object parameters = null)
        {
            new StoredProcedureDbCommand(session, procedureName, parameters).Execute();
        }

        public static void Execute(IsolationLevel isolationLevel, string procedureName, object parameters = null)
        {
            new StoredProcedureDbCommand(procedureName, parameters).Execute(isolationLevel);
        }

        public static void Execute(DbSessionBase session, IsolationLevel isolationLevel, string procedureName, object parameters = null)
        {
            new StoredProcedureDbCommand(session, procedureName, parameters).Execute(isolationLevel);
        }

        #endregion


        #region --- Async Commands ---

        public static async Task ExecuteAsync(string procedureName, object parameters = null)
        {
            await new StoredProcedureDbCommand(procedureName, parameters).ExecuteAsync();
        }

        public static async Task ExecuteAsync(DbSessionBase session, string procedureName, object parameters = null)
        {
            await new StoredProcedureDbCommand(session, procedureName, parameters).ExecuteAsync();
        }

        public static async Task ExecuteAsync(IsolationLevel isolationLevel, string procedureName, object parameters = null)
        {
            await new StoredProcedureDbCommand(procedureName, parameters).ExecuteAsync(isolationLevel);
        }

        public static async Task ExecuteAsync(DbSessionBase session, IsolationLevel isolationLevel, string procedureName, object parameters = null)
        {
            await new StoredProcedureDbCommand(session, procedureName, parameters).ExecuteAsync(isolationLevel);
        }

        #endregion
    }
}
