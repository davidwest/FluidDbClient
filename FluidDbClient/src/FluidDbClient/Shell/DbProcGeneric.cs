using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FluidDbClient.Shell
{
    public static class DbProc<TDatabase> where TDatabase : Database
    {
        // TODO: add all versions that take DbConnection or DbTransaction

        #region --- Synchronous Queries ---

        public static T GetScalar<T>(string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).GetScalar<T>();
        }

        public static T GetScalar<T>(DbSession<TDatabase> session, string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery<TDatabase>(session, procedureName, parameters).GetScalar<T>();
        }

        public static T GetScalar<T>(string procedureName, T dbNullSubstitute, object parameters = null)
        {
            return new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).GetScalar(dbNullSubstitute);
        }

        public static T GetScalar<T>(DbSession<TDatabase> session, string procedureName, T dbNullSubstitute, object parameters = null)
        {
            return new StoredProcedureDbQuery<TDatabase>(session, procedureName, parameters).GetScalar(dbNullSubstitute);
        }



        public static IDataRecord GetRecord(string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).GetRecord();
        }

        public static IDataRecord GetRecord(DbSession<TDatabase> session, string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery<TDatabase>(session, procedureName, parameters).GetRecord();
        }



        public static IEnumerable<IDataRecord> GetResultSet(string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).GetResultSet();
        }

        public static IEnumerable<IDataRecord> GetResultSet(DbSession<TDatabase> session, string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery<TDatabase>(session, procedureName, parameters).GetResultSet();
        }



        public static void ProcessResultSets(string procedureName, object parameters, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).ProcessResultSets(processes);
        }

        public static void ProcessResultSets(DbSession<TDatabase> session, string procedureName, object parameters, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new StoredProcedureDbQuery<TDatabase>(session, procedureName, parameters).ProcessResultSets(processes);
        }

        public static void ProcessResultSets(string procedureName, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new StoredProcedureDbQuery<TDatabase>(procedureName).ProcessResultSets(processes);
        }

        public static void ProcessResultSets(DbSession<TDatabase> session, string procedureName, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new StoredProcedureDbQuery<TDatabase>(session, procedureName).ProcessResultSets(processes);
        }


        public static List<T>[] CollectResultSets<T>(int resultCount, string procedureName, Func<IDataRecord, T> map, object parameters = null)
        {
            return new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).CollectResultSets(resultCount, map);
        }

        public static List<T>[] CollectResultSets<T>(DbSession<TDatabase> session, int resultCount, string procedureName, Func<IDataRecord, T> map, object parameters = null)
        {
            return new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).CollectResultSets(resultCount, map);
        }


        public static List<IDataRecord>[] CollectResultSets(int resultCount, string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).CollectResultSets(resultCount);
        }

        public static List<IDataRecord>[] CollectResultSets(DbSession<TDatabase> session, int resultCount, string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).CollectResultSets(resultCount);
        }


        public static List<Dictionary<string, object>>[] CollectResultSetsAsDictionaries(int resultCount, string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).CollectResultSetsAsDictionaries(resultCount);
        }

        public static List<Dictionary<string, object>>[] CollectResultSetsAsDictionaries(DbSession<TDatabase> session, int resultCount, string procedureName, object parameters = null)
        {
            return new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).CollectResultSetsAsDictionaries(resultCount);
        }

        #endregion


        #region --- Async Queries ---

        public static async Task<T> GetScalarAsync<T>(string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).GetScalarAsync<T>();
        }

        public static async Task<T> GetScalarAsync<T>(DbSession<TDatabase> session, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery<TDatabase>(session, procedureName, parameters).GetScalarAsync<T>();
        }

        public static async Task<T> GetScalarAsync<T>(string procedureName, T dbNullSubstitute, object parameters = null)
        {
            return await new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).GetScalarAsync(dbNullSubstitute);
        }

        public static async Task<T> GetScalarAsync<T>(DbSession<TDatabase> session, T dbNullSubstitute, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery<TDatabase>(session, procedureName, parameters).GetScalarAsync(dbNullSubstitute);
        }



        public static async Task<IDataRecord> GetRecordAsync(string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).GetRecordAsync();
        }

        public static async Task<IDataRecord> GetRecordAsync(DbSession<TDatabase> session, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery<TDatabase>(session, procedureName, parameters).GetRecordAsync();
        }



        public static async Task ProcessResultSetAsync(string procedureName, Action<IDataRecord> process, object parameters = null)
        {
            await new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).ProcessResultSetAsync(process);
        }

        public static async Task ProcessResultSetAsync(DbSession<TDatabase> session, string procedureName, Action<IDataRecord> process, object parameters = null)
        {
            await new StoredProcedureDbQuery<TDatabase>(session, procedureName, parameters).ProcessResultSetAsync(process);
        }


        public static async Task<List<T>> CollectResultSetAsync<T>(string procedureName, Func<IDataRecord, T> map, object parameters = null)
        {
            return await new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).CollectResultSetAsync(map);
        }

        public static async Task<List<T>> CollectResultSetAsync<T>(DbSession<TDatabase> session, string procedureName, Func<IDataRecord, T> map, object parameters = null)
        {
            return await new StoredProcedureDbQuery<TDatabase>(session, procedureName, parameters).CollectResultSetAsync(map);
        }


        public static async Task<List<IDataRecord>> CollectResultSetAsync(string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).CollectResultSetAsync();
        }

        public static async Task<List<IDataRecord>> CollectResultSetAsync(DbSession<TDatabase> session, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery<TDatabase>(session, procedureName, parameters).CollectResultSetAsync();
        }


        public static async Task<List<Dictionary<string, object>>> CollectResultSetAsDictionariesAsync(string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).CollectResultSetAsDictionariesAsync();
        }

        public static async Task<List<Dictionary<string, object>>> CollectResultSetAsDictionariesAsync(DbSession<TDatabase> session, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery<TDatabase>(session, procedureName, parameters).CollectResultSetAsDictionariesAsync();
        }



        public static async Task ProcessResultSetsAsync(string procedureName, params Action<IDataRecord>[] processes)
        {
            await new StoredProcedureDbQuery<TDatabase>(procedureName).ProcessResultSetsAsync(processes);
        }

        public static async Task ProcessResultSetsAsync(DbSession<TDatabase> session, string procedureName, params Action<IDataRecord>[] processes)
        {
            await new StoredProcedureDbQuery<TDatabase>(session, procedureName).ProcessResultSetsAsync(processes);
        }

        public static async Task ProcessResultSetsAsync(string procedureName, object parameters, params Action<IDataRecord>[] processes)
        {
            await new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).ProcessResultSetsAsync(processes);
        }

        public static async Task ProcessResultSetsAsync(DbSession<TDatabase> session, string procedureName, object parameters, params Action<IDataRecord>[] processes)
        {
            await new StoredProcedureDbQuery<TDatabase>(session, procedureName, parameters).ProcessResultSetsAsync(processes);
        }


        public static async Task<List<T>[]> CollectResultSetsAsync<T>(int resultCount, Func<IDataRecord, T> map, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).CollectResultSetsAsync(resultCount, map);
        }

        public static async Task<List<T>[]> CollectResultSetsAsync<T>(DbSession<TDatabase> session, int resultCount, Func<IDataRecord, T> map, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery<TDatabase>(session, procedureName, parameters).CollectResultSetsAsync(resultCount, map);
        }


        public static async Task<List<IDataRecord>[]> CollectResultSetsAsync(int resultCount, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).CollectResultSetsAsync(resultCount);
        }
        
        public static async Task<List<IDataRecord>[]> CollectResultSetsAsync(DbSession<TDatabase> session, int resultCount, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery<TDatabase>(session, procedureName, parameters).CollectResultSetsAsync(resultCount);
        }


        public static async Task<List<Dictionary<string, object>>[]> CollectResultSetsAsDictionariesAsync(int resultCount, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery<TDatabase>(procedureName, parameters).CollectResultSetsAsDictionariesAsync(resultCount);
        }
        
        public static async Task<List<Dictionary<string, object>>[]> CollectResultSetsAsDictionariesAsync(DbSession<TDatabase> session, int resultCount, string procedureName, object parameters = null)
        {
            return await new StoredProcedureDbQuery<TDatabase>(session, procedureName, parameters).CollectResultSetsAsDictionariesAsync(resultCount);
        }

        #endregion


        #region --- Synchronous Commands ---

        public static void Execute(string procedureName, object parameters = null)
        {
            new StoredProcedureDbCommand<TDatabase>(procedureName, parameters).Execute();
        }

        public static void Execute(DbSession<TDatabase> session, string procedureName, object parameters = null)
        {
            new StoredProcedureDbCommand<TDatabase>(session, procedureName, parameters).Execute();
        }

        public static void Execute(IsolationLevel isolationLevel, string procedureName, object parameters = null)
        {
            new StoredProcedureDbCommand<TDatabase>(procedureName, parameters).Execute(isolationLevel);
        }

        public static void Execute(DbSession<TDatabase> session, IsolationLevel isolationLevel, string procedureName, object parameters = null)
        {
            new StoredProcedureDbCommand<TDatabase>(session, procedureName, parameters).Execute(isolationLevel);
        }

        #endregion


        #region --- Async Commands ---

        public static async Task ExecuteAsync(string procedureName, object parameters = null)
        {
            await new StoredProcedureDbCommand<TDatabase>(procedureName, parameters).ExecuteAsync();
        }

        public static async Task ExecuteAsync(DbSession<TDatabase> session, string procedureName, object parameters = null)
        {
            await new StoredProcedureDbCommand<TDatabase>(session, procedureName, parameters).ExecuteAsync();
        }

        public static async Task ExecuteAsync(IsolationLevel isolationLevel, string procedureName, object parameters = null)
        {
            await new StoredProcedureDbCommand<TDatabase>(procedureName, parameters).ExecuteAsync(isolationLevel);
        }

        public static async Task ExecuteAsync(DbSession<TDatabase> session, IsolationLevel isolationLevel, string procedureName, object parameters = null)
        {
            await new StoredProcedureDbCommand<TDatabase>(session, procedureName, parameters).ExecuteAsync(isolationLevel);
        }

        #endregion
    }
}
