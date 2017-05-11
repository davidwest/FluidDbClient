using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FluidDbClient.Shell
{
    public static class Db<TDatabase> where TDatabase : Database
    {
        public static DbConnection CreateConnection()
        {
            return DbRegistry.GetDatabase<TDatabase>().Provider.CreateConnection();
        }

        // TODO: add all versions that take DbConnection or DbTransaction

        #region --- Synchronous Queries ---

        public static T GetScalar<T>(string script, object parameters = null)
        {
            return new ScriptDbQuery<TDatabase>(script, parameters).GetScalar<T>();
        }

        public static T GetScalar<T>(DbSession<TDatabase> session, string script, object parameters = null)
        {
            return new ScriptDbQuery<TDatabase>(session, script, parameters).GetScalar<T>();
        }

        public static T GetScalar<T>(string script, T dbNullSubstitute, object parameters = null)
        {
            return new ScriptDbQuery<TDatabase>(script, parameters).GetScalar(dbNullSubstitute);
        }

        public static T GetScalar<T>(DbSession<TDatabase> session, string script, T dbNullSubstitute, object parameters = null)
        {
            return new ScriptDbQuery<TDatabase>(session, script, parameters).GetScalar(dbNullSubstitute);
        }



        public static IDataRecord GetRecord(string script, object parameters = null)
        {
            return new ScriptDbQuery<TDatabase>(script, parameters).GetRecord();
        }

        public static IDataRecord GetRecord(DbSession<TDatabase> session, string script, object parameters = null)
        {
            return new ScriptDbQuery<TDatabase>(session, script, parameters).GetRecord();
        }



        public static IEnumerable<IDataRecord> GetResultSet(string script, object parameters = null)
        {
            return new ScriptDbQuery<TDatabase>(script, parameters).GetResultSet();
        }

        public static IEnumerable<IDataRecord> GetResultSet(DbSession<TDatabase> session, string script, object parameters = null)
        {
            return new ScriptDbQuery<TDatabase>(session, script, parameters).GetResultSet();
        }



        public static void ProcessResultSets(string script, object parameters, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new ScriptDbQuery<TDatabase>(script, parameters).ProcessResultSets(processes);
        }

        public static void ProcessResultSets(DbSession<TDatabase> session, string script, object parameters, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new ScriptDbQuery<TDatabase>(session, script, parameters).ProcessResultSets(processes);
        }

        public static void ProcessResultSets(string script, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new ScriptDbQuery<TDatabase>(script).ProcessResultSets(processes);
        }

        public static void ProcessResultSets(DbSession<TDatabase> session, string script, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new ScriptDbQuery<TDatabase>(session, script).ProcessResultSets(processes);
        }


        public static List<T>[] CollectResultSets<T>(int resultCount, string script, Func<IDataRecord, T> map, object parameters = null)
        {
            return new ScriptDbQuery<TDatabase>(script, parameters).CollectResultSets(resultCount, map);
        }

        public static List<T>[] CollectResultSets<T>(DbSession<TDatabase> session, int resultCount, string script, Func<IDataRecord, T> map, object parameters = null)
        {
            return new ScriptDbQuery<TDatabase>(script, parameters).CollectResultSets(resultCount, map);
        }


        public static List<IDataRecord>[] CollectResultSets(int resultCount, string script, object parameters = null)
        {
            return new ScriptDbQuery<TDatabase>(script, parameters).CollectResultSets(resultCount);
        }

        public static List<IDataRecord>[] CollectResultSets(DbSession<TDatabase> session, int resultCount, string script, object parameters = null)
        {
            return new ScriptDbQuery<TDatabase>(script, parameters).CollectResultSets(resultCount);
        }


        public static List<Dictionary<string, object>>[] CollectResultSetsAsDictionaries(int resultCount, string script, object parameters = null)
        {
            return new ScriptDbQuery<TDatabase>(script, parameters).CollectResultSetsAsDictionaries(resultCount);
        }

        public static List<Dictionary<string, object>>[] CollectResultSetsAsDictionaries(DbSession<TDatabase> session, int resultCount, string script, object parameters = null)
        {
            return new ScriptDbQuery<TDatabase>(script, parameters).CollectResultSetsAsDictionaries(resultCount);
        }

        #endregion


        #region --- Async Queries ---

        public static async Task<T> GetScalarAsync<T>(string script, object parameters = null)
        {
            return await new ScriptDbQuery<TDatabase>(script, parameters).GetScalarAsync<T>();
        }

        public static async Task<T> GetScalarAsync<T>(DbSession<TDatabase> session, string script, object parameters = null)
        {
            return await new ScriptDbQuery<TDatabase>(session, script, parameters).GetScalarAsync<T>();
        }

        public static async Task<T> GetScalarAsync<T>(string script, T dbNullSubstitute, object parameters = null)
        {
            return await new ScriptDbQuery<TDatabase>(script, parameters).GetScalarAsync(dbNullSubstitute);
        }

        public static async Task<T> GetScalarAsync<T>(DbSession<TDatabase> session, T dbNullSubstitute, string script, object parameters = null)
        {
            return await new ScriptDbQuery<TDatabase>(session, script, parameters).GetScalarAsync(dbNullSubstitute);
        }



        public static async Task<IDataRecord> GetRecordAsync(string script, object parameters = null)
        {
            return await new ScriptDbQuery<TDatabase>(script, parameters).GetRecordAsync();
        }

        public static async Task<IDataRecord> GetRecordAsync(DbSession<TDatabase> session, string script, object parameters = null)
        {
            return await new ScriptDbQuery<TDatabase>(session, script, parameters).GetRecordAsync();
        }



        public static async Task ProcessResultSetAsync(string script, Action<IDataRecord> process, object parameters = null)
        {
            await new ScriptDbQuery<TDatabase>(script, parameters).ProcessResultSetAsync(process);
        }

        public static async Task ProcessResultSetAsync(DbSession<TDatabase> session, string script, Action<IDataRecord> process, object parameters = null)
        {
            await new ScriptDbQuery<TDatabase>(session, script, parameters).ProcessResultSetAsync(process);
        }


        public static async Task<List<T>> CollectResultSetAsync<T>(string script, Func<IDataRecord, T> map, object parameters = null)
        {
            return await new ScriptDbQuery<TDatabase>(script, parameters).CollectResultSetAsync(map);
        }

        public static async Task<List<T>> CollectResultSetAsync<T>(DbSession<TDatabase> session, string script, Func<IDataRecord, T> map, object parameters = null)
        {
            return await new ScriptDbQuery<TDatabase>(session, script, parameters).CollectResultSetAsync(map);
        }


        public static async Task<List<IDataRecord>> CollectResultSetAsync(string script, object parameters = null)
        {
            return await new ScriptDbQuery<TDatabase>(script, parameters).CollectResultSetAsync();
        }

        public static async Task<List<IDataRecord>> CollectResultSetAsync(DbSession<TDatabase> session, string script, object parameters = null)
        {
            return await new ScriptDbQuery<TDatabase>(session, script, parameters).CollectResultSetAsync();
        }


        public static async Task<List<Dictionary<string, object>>> CollectResultSetAsDictionariesAsync(string script, object parameters = null)
        {
            return await new ScriptDbQuery<TDatabase>(script, parameters).CollectResultSetAsDictionariesAsync();
        }

        public static async Task<List<Dictionary<string, object>>> CollectResultSetAsDictionariesAsync(DbSession<TDatabase> session, string script, object parameters = null)
        {
            return await new ScriptDbQuery<TDatabase>(session, script, parameters).CollectResultSetAsDictionariesAsync();
        }



        public static async Task ProcessResultSetsAsync(string script, params Action<IDataRecord>[] processes)
        {
            await new ScriptDbQuery<TDatabase>(script).ProcessResultSetsAsync(processes);
        }

        public static async Task ProcessResultSetsAsync(DbSession<TDatabase> session, string script, params Action<IDataRecord>[] processes)
        {
            await new ScriptDbQuery<TDatabase>(session, script).ProcessResultSetsAsync(processes);
        }

        public static async Task ProcessResultSetsAsync(string script, object parameters, params Action<IDataRecord>[] processes)
        {
            await new ScriptDbQuery<TDatabase>(script, parameters).ProcessResultSetsAsync(processes);
        }

        public static async Task ProcessResultSetsAsync(DbSession<TDatabase> session, string script, object parameters, params Action<IDataRecord>[] processes)
        {
            await new ScriptDbQuery<TDatabase>(session, script, parameters).ProcessResultSetsAsync(processes);
        }


        public static async Task<List<T>[]> CollectResultSetsAsync<T>(int resultCount, Func<IDataRecord, T> map, string script, object parameters = null)
        {
            return await new ScriptDbQuery<TDatabase>(script, parameters).CollectResultSetsAsync(resultCount, map);
        }

        public static async Task<List<T>[]> CollectResultSetsAsync<T>(DbSession<TDatabase> session, int resultCount, Func<IDataRecord, T> map, string script, object parameters = null)
        {
            return await new ScriptDbQuery<TDatabase>(session, script, parameters).CollectResultSetsAsync(resultCount, map);
        }


        public static async Task<List<IDataRecord>[]> CollectResultSetsAsync(int resultCount, string script, object parameters = null)
        {
            return await new ScriptDbQuery<TDatabase>(script, parameters).CollectResultSetsAsync(resultCount);
        }
        
        public static async Task<List<IDataRecord>[]> CollectResultSetsAsync(DbSession<TDatabase> session, int resultCount, string script, object parameters = null)
        {
            return await new ScriptDbQuery<TDatabase>(session, script, parameters).CollectResultSetsAsync(resultCount);
        }


        public static async Task<List<Dictionary<string, object>>[]> CollectResultSetsAsDictionariesAsync(int resultCount, string script, object parameters = null)
        {
            return await new ScriptDbQuery<TDatabase>(script, parameters).CollectResultSetsAsDictionariesAsync(resultCount);
        }
        
        public static async Task<List<Dictionary<string, object>>[]> CollectResultSetsAsDictionariesAsync(DbSession<TDatabase> session, int resultCount, string script, object parameters = null)
        {
            return await new ScriptDbQuery<TDatabase>(session, script, parameters).CollectResultSetsAsDictionariesAsync(resultCount);
        }

        #endregion


        #region --- Synchronous Commands ---

        public static void Execute(string script, object parameters = null)
        {
            new ScriptDbCommand<TDatabase>(script, parameters).Execute();
        }

        public static void Execute(DbSession<TDatabase> session, string script, object parameters = null)
        {
            new ScriptDbCommand<TDatabase>(session, script, parameters).Execute();
        }

        public static void Execute(IsolationLevel isolationLevel, string script, object parameters = null)
        {
            new ScriptDbCommand<TDatabase>(script, parameters).Execute(isolationLevel);
        }

        public static void Execute(DbSession<TDatabase> session, IsolationLevel isolationLevel, string script, object parameters = null)
        {
            new ScriptDbCommand<TDatabase>(session, script, parameters).Execute(isolationLevel);
        }

        #endregion


        #region --- Async Commands ---

        public static async Task ExecuteAsync(string script, object parameters = null)
        {
            await new ScriptDbCommand<TDatabase>(script, parameters).ExecuteAsync();
        }

        public static async Task ExecuteAsync(DbSession<TDatabase> session, string script, object parameters = null)
        {
            await new ScriptDbCommand<TDatabase>(session, script, parameters).ExecuteAsync();
        }

        public static async Task ExecuteAsync(IsolationLevel isolationLevel, string script, object parameters = null)
        {
            await new ScriptDbCommand<TDatabase>(script, parameters).ExecuteAsync(isolationLevel);
        }

        public static async Task ExecuteAsync(DbSession<TDatabase> session, IsolationLevel isolationLevel, string script, object parameters = null)
        {
            await new ScriptDbCommand<TDatabase>(session, script, parameters).ExecuteAsync(isolationLevel);
        }

        #endregion
    }
}
