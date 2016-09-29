
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FluidDbClient.Shell
{
    public static class DbProc
    {
        #region --- Synchronous Queries ---

        public static T GetScalar<T>(string script, object parameters = null)
        {
            return new StoredProcedureDbQuery(script, parameters).GetScalar<T>();
        }

        public static T GetScalar<T>(DbSessionBase session, string script, object parameters = null)
        {
            return new StoredProcedureDbQuery(session, script, parameters).GetScalar<T>();
        }

        public static T GetScalar<T>(string script, T dbNullSubstitute, object parameters = null)
        {
            return new StoredProcedureDbQuery(script, parameters).GetScalar(dbNullSubstitute);
        }

        public static T GetScalar<T>(DbSessionBase session, string script, T dbNullSubstitute, object parameters = null)
        {
            return new StoredProcedureDbQuery(session, script, parameters).GetScalar(dbNullSubstitute);
        }



        public static IDataRecord GetRecord(string script, object parameters = null)
        {
            return new StoredProcedureDbQuery(script, parameters).GetRecord();
        }

        public static IDataRecord GetRecord(DbSessionBase session, string script, object parameters = null)
        {
            return new StoredProcedureDbQuery(session, script, parameters).GetRecord();
        }



        public static IEnumerable<IDataRecord> GetResultSet(string script, object parameters = null)
        {
            return new StoredProcedureDbQuery(script, parameters).GetResultSet();
        }

        public static IEnumerable<IDataRecord> GetResultSet(DbSessionBase session, string script, object parameters = null)
        {
            return new StoredProcedureDbQuery(session, script, parameters).GetResultSet();
        }



        public static void ProcessResultSets(string script, object parameters, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new StoredProcedureDbQuery(script, parameters).ProcessResultSets(processes);
        }

        public static void ProcessResultSets(DbSessionBase session, string script, object parameters, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new StoredProcedureDbQuery(session, script, parameters).ProcessResultSets(processes);
        }

        public static void ProcessResultSets(string script, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new StoredProcedureDbQuery(script).ProcessResultSets(processes);
        }

        public static void ProcessResultSets(DbSessionBase session, string script, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new StoredProcedureDbQuery(session, script).ProcessResultSets(processes);
        }
        


        public static List<IDataRecord>[] CollectResultSets(int resultCount, string script, object parameters = null)
        {
            return new StoredProcedureDbQuery(script, parameters).CollectResultSets(resultCount);
        }

        public static List<IDataRecord>[] CollectResultSets(DbSessionBase session, int resultCount, string script, object parameters = null)
        {
            return new StoredProcedureDbQuery(script, parameters).CollectResultSets(resultCount);
        }
        
        public static List<dynamic>[] CollectResultSetsDynamic(int resultCount, string script, object parameters = null)
        {
            return new StoredProcedureDbQuery(script, parameters).CollectResultSetsDynamic(resultCount);
        }

        public static List<dynamic>[] CollectResultSetsDynamic(DbSessionBase session, int resultCount, string script, object parameters = null)
        {
            return new StoredProcedureDbQuery(script, parameters).CollectResultSetsDynamic(resultCount);
        }

        #endregion


        #region --- Async Queries ---

        public static async Task<T> GetScalarAsync<T>(string script, object parameters = null)
        {
            return await new StoredProcedureDbQuery(script, parameters).GetScalarAsync<T>();
        }

        public static async Task<T> GetScalarAsync<T>(DbSessionBase session, string script, object parameters = null)
        {
            return await new StoredProcedureDbQuery(session, script, parameters).GetScalarAsync<T>();
        }

        public static async Task<T> GetScalarAsync<T>(string script, T dbNullSubstitute, object parameters = null)
        {
            return await new StoredProcedureDbQuery(script, parameters).GetScalarAsync(dbNullSubstitute);
        }

        public static async Task<T> GetScalarAsync<T>(DbSessionBase session, T dbNullSubstitute, string script, object parameters = null)
        {
            return await new StoredProcedureDbQuery(session, script, parameters).GetScalarAsync(dbNullSubstitute);
        }



        public static async Task<IDataRecord> GetRecordAsync(string script, object parameters = null)
        {
            return await new StoredProcedureDbQuery(script, parameters).GetRecordAsync();
        }

        public static async Task<IDataRecord> GetRecordAsync(DbSessionBase session, string script, object parameters = null)
        {
            return await new StoredProcedureDbQuery(session, script, parameters).GetRecordAsync();
        }



        public static async Task ProcessResultSetAsync(string script, Action<IDataRecord> process)
        {
            await new StoredProcedureDbQuery(script).ProcessResultSetAsync(process);
        }

        public static async Task ProcessResultSetAsync(DbSessionBase session, string script, Action<IDataRecord> process)
        {
            await new StoredProcedureDbQuery(session, script).ProcessResultSetAsync(process);
        }

        public static async Task ProcessResultSetAsync(string script, object parameters, Action<IDataRecord> process)
        {
            await new StoredProcedureDbQuery(script, parameters).ProcessResultSetAsync(process);
        }

        public static async Task ProcessResultSetAsync(DbSessionBase session, string script, object parameters, Action<IDataRecord> process)
        {
            await new StoredProcedureDbQuery(session, script, parameters).ProcessResultSetAsync(process);
        }



        public static async Task<List<IDataRecord>> CollectResultSetAsync(string script, object parameters = null)
        {
            return await new StoredProcedureDbQuery(script, parameters).CollectResultSetAsync();
        }

        public static async Task<List<IDataRecord>> CollectResultSetAsync(DbSessionBase session, string script, object parameters = null)
        {
            return await new StoredProcedureDbQuery(session, script, parameters).CollectResultSetAsync();
        }



        public static async Task<List<T>> CollectResultSetAsync<T>(string script, Func<IDataRecord, T> map)
        {
            return await new StoredProcedureDbQuery(script).CollectResultSetAsync(map);
        }

        public static async Task<List<T>> CollectResultSetAsync<T>(DbSessionBase session, string script, Func<IDataRecord, T> map)
        {
            return await new StoredProcedureDbQuery(session, script).CollectResultSetAsync(map);
        }

        public static async Task<List<T>> CollectResultSetAsync<T>(string script, object parameters, Func<IDataRecord, T> map)
        {
            return await new StoredProcedureDbQuery(script, parameters).CollectResultSetAsync(map);
        }

        public static async Task<List<T>> CollectResultSetAsync<T>(DbSessionBase session, string script, object parameters, Func<IDataRecord, T> map)
        {
            return await new StoredProcedureDbQuery(session, script, parameters).CollectResultSetAsync(map);
        }



        public static async Task<List<dynamic>> CollectResultSetDynamicAsync(string script, object parameters = null)
        {
            return await new StoredProcedureDbQuery(script, parameters).CollectResultSetDynamicAsync();
        }

        public static async Task<List<dynamic>> CollectResultSetDynamicAsync(DbSessionBase session, string script, object parameters = null)
        {
            return await new StoredProcedureDbQuery(session, script, parameters).CollectResultSetDynamicAsync();
        }



        public static async Task ProcessResultSetsAsync(string script, params Action<IDataRecord>[] processes)
        {
            await new StoredProcedureDbQuery(script).ProcessResultSetsAsync(processes);
        }

        public static async Task ProcessResultSetsAsync(DbSessionBase session, string script, params Action<IDataRecord>[] processes)
        {
            await new StoredProcedureDbQuery(session, script).ProcessResultSetsAsync(processes);
        }

        public static async Task ProcessResultSetsAsync(string script, object parameters, params Action<IDataRecord>[] processes)
        {
            await new StoredProcedureDbQuery(script, parameters).ProcessResultSetsAsync(processes);
        }

        public static async Task ProcessResultSetsAsync(DbSessionBase session, string script, object parameters, params Action<IDataRecord>[] processes)
        {
            await new StoredProcedureDbQuery(session, script, parameters).ProcessResultSetsAsync(processes);
        }



        public static async Task<List<IDataRecord>[]> CollectResultSetsAsync(string script, int resultCount)
        {
            return await new StoredProcedureDbQuery(script).CollectResultSetsAsync(resultCount);
        }

        public static async Task<List<IDataRecord>[]> CollectResultSetsAsync(DbSessionBase session, string script, int resultCount)
        {
            return await new StoredProcedureDbQuery(session, script).CollectResultSetsAsync(resultCount);
        }

        public static async Task<List<IDataRecord>[]> CollectResultSetsAsync(string script, object parameters, int resultCount)
        {
            return await new StoredProcedureDbQuery(script, parameters).CollectResultSetsAsync(resultCount);
        }

        public static async Task<List<IDataRecord>[]> CollectResultSetsAsync(DbSessionBase session, string script, object parameters, int resultCount)
        {
            return await new StoredProcedureDbQuery(session, script, parameters).CollectResultSetsAsync(resultCount);
        }



        public static async Task<List<dynamic>[]> CollectResultSetsDynamicAsync(string script, int resultCount)
        {
            return await new StoredProcedureDbQuery(script).CollectResultSetsDynamicAsync(resultCount);
        }

        public static async Task<List<dynamic>[]> CollectResultSetsDynamicAsync(DbSessionBase session, string script, int resultCount)
        {
            return await new StoredProcedureDbQuery(session, script).CollectResultSetsDynamicAsync(resultCount);
        }

        public static async Task<List<dynamic>[]> CollectResultSetsDynamicAsync(string script, object parameters, int resultCount)
        {
            return await new StoredProcedureDbQuery(script, parameters).CollectResultSetsDynamicAsync(resultCount);
        }

        public static async Task<List<dynamic>[]> CollectResultSetsDynamicAsync(DbSessionBase session, string script, object parameters, int resultCount)
        {
            return await new StoredProcedureDbQuery(session, script, parameters).CollectResultSetsDynamicAsync(resultCount);
        }

        #endregion 


        #region --- Synchronous Commands ---

        public static void Execute(string script, object parameters = null)
        {
            new StoredProcedureDbCommand(script, parameters).Execute();
        }

        public static void Execute(DbSessionBase session, string script, object parameters = null)
        {
            new StoredProcedureDbCommand(session, script, parameters).Execute();
        }

        public static void Execute(IsolationLevel isolationLevel, string script, object parameters = null)
        {
            new StoredProcedureDbCommand(script, parameters).Execute(isolationLevel);
        }

        public static void Execute(DbSessionBase session, IsolationLevel isolationLevel, string script, object parameters = null)
        {
            new StoredProcedureDbCommand(session, script, parameters).Execute(isolationLevel);
        }

        #endregion


        #region --- Async Commands ---

        public static async Task ExecuteAsync(string script, object parameters = null)
        {
            await new StoredProcedureDbCommand(script, parameters).ExecuteAsync();
        }

        public static async Task ExecuteAsync(DbSessionBase session, string script, object parameters = null)
        {
            await new StoredProcedureDbCommand(session, script, parameters).ExecuteAsync();
        }

        public static async Task ExecuteAsync(IsolationLevel isolationLevel, string script, object parameters = null)
        {
            await new StoredProcedureDbCommand(script, parameters).ExecuteAsync(isolationLevel);
        }

        public static async Task ExecuteAsync(DbSessionBase session, IsolationLevel isolationLevel, string script, object parameters = null)
        {
            await new StoredProcedureDbCommand(session, script, parameters).ExecuteAsync(isolationLevel);
        }

        #endregion
    }
}
