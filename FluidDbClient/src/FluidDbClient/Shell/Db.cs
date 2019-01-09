using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FluidDbClient.Shell
{
    public static class Db
    {
        public static DbConnection CreateConnection()
        {
            return DbRegistry.GetDatabase().Provider.CreateConnection();
        }

        // TODO: add all versions that take DbConnection or DbTransaction

        #region --- Synchronous Queries ---

        public static T GetScalar<T>(string script, object parameters = null)
        {
            return new ScriptDbQuery(script, parameters).GetScalar<T>();
        }

        public static T GetScalar<T>(DbSessionBase session, string script, object parameters = null)
        {
            return new ScriptDbQuery(session, script, parameters).GetScalar<T>();
        }

        public static T GetScalar<T>(string script, T dbNullSubstitute, object parameters = null)
        {
            return new ScriptDbQuery(script, parameters).GetScalar(dbNullSubstitute);
        }

        public static T GetScalar<T>(DbSessionBase session, string script, T dbNullSubstitute, object parameters = null)
        {
            return new ScriptDbQuery(session, script, parameters).GetScalar(dbNullSubstitute);
        }
        
        public static T GetScalar<T>(DbConnection connection, string script, object parameters = null)
        {
            return new ScriptDbQuery(connection, script, parameters).GetScalar<T>();
        }

        public static T GetScalar<T>(DbTransaction transaction, string script, object parameters = null)
        {
            return new ScriptDbQuery(transaction, script, parameters).GetScalar<T>();
        }



        public static IDataRecord GetRecord(string script, object parameters = null)
        {
            return new ScriptDbQuery(script, parameters).GetRecord();
        }

        public static IDataRecord GetRecord(DbSessionBase session, string script, object parameters = null)
        {
            return new ScriptDbQuery(session, script, parameters).GetRecord();
        }

        public static IDataRecord GetRecord(DbConnection connection, string script, object parameters = null)
        {
            return new ScriptDbQuery(connection, script, parameters).GetRecord();
        }

        public static IDataRecord GetRecord(DbTransaction transaction, string script, object parameters = null)
        {
            return new ScriptDbQuery(transaction, script, parameters).GetRecord();
        }



        public static IEnumerable<IDataRecord> GetResultSet(string script, object parameters = null)
        {
            return new ScriptDbQuery(script, parameters).GetResultSet();
        }

        public static IEnumerable<IDataRecord> GetResultSet(DbSessionBase session, string script, object parameters = null)
        {
            return new ScriptDbQuery(session, script, parameters).GetResultSet();
        }

        public static IEnumerable<IDataRecord> GetResultSet(DbConnection connection, string script, object parameters = null)
        {
            return new ScriptDbQuery(connection, script, parameters).GetResultSet();
        }

        public static IEnumerable<IDataRecord> GetResultSet(DbTransaction transaction, string script, object parameters = null)
        {
            return new ScriptDbQuery(transaction, script, parameters).GetResultSet();
        }


        public static DataTable GetDataTable(string script, string tableName, object parameters = null)
        {
            return new ScriptDbQuery(script, parameters).GetDataTable(tableName);
        }

        public static DataTable GetDataTable(DbSessionBase session, string script, string tableName, object parameters = null)
        {
            return new ScriptDbQuery(session, script, parameters).GetDataTable(tableName);
        }

        public static DataTable GetDataTable(DbConnection connection, string script, string tableName, object parameters = null)
        {
            return new ScriptDbQuery(connection, script, parameters).GetDataTable(tableName);
        }
        
        public static DataTable GetDataTable(DbTransaction transaction, string script, string tableName, object parameters = null)
        {
            return new ScriptDbQuery(transaction, script, parameters).GetDataTable(tableName);
        }


        public static DataSet GetDataSet(string script, string[] tableNames, object parameters = null)
        {
            return new ScriptDbQuery(script, parameters).GetDataSet(tableNames);
        }

        public static DataSet GetDataSet(DbSessionBase session, string script, string[] tableNames, object parameters = null)
        {
            return new ScriptDbQuery(session, script, parameters).GetDataSet(tableNames);
        }

        public static DataSet GetDataSet(DbConnection connection, string script, string[] tableNames, object parameters = null)
        {
            return new ScriptDbQuery(connection, script, parameters).GetDataSet(tableNames);
        }

        public static DataSet GetDataSet(DbTransaction transaction, string script, string[] tableNames, object parameters = null)
        {
            return new ScriptDbQuery(transaction, script, parameters).GetDataSet(tableNames);
        }


        public static void ProcessResultSets(string script, object parameters, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new ScriptDbQuery(script, parameters).ProcessResultSets(processes);
        }

        public static void ProcessResultSets(DbSessionBase session, string script, object parameters, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new ScriptDbQuery(session, script, parameters).ProcessResultSets(processes);
        }

        public static void ProcessResultSets(DbConnection connection, string script, object parameters, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new ScriptDbQuery(connection, script, parameters).ProcessResultSets(processes);
        }

        public static void ProcessResultSets(DbTransaction transaction, string script, object parameters, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new ScriptDbQuery(transaction, script, parameters).ProcessResultSets(processes);
        }

        public static void ProcessResultSets(string script, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new ScriptDbQuery(script).ProcessResultSets(processes);
        }

        public static void ProcessResultSets(DbSessionBase session, string script, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new ScriptDbQuery(session, script).ProcessResultSets(processes);
        }

        public static void ProcessResultSets(DbConnection connection, string script, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new ScriptDbQuery(connection, script).ProcessResultSets(processes);
        }

        public static void ProcessResultSets(DbTransaction transaction, string script, params Action<IEnumerable<IDataRecord>>[] processes)
        {
            new ScriptDbQuery(transaction, script).ProcessResultSets(processes);
        }


        // ******** TODO: start adding new versions here: *************

        public static List<T>[] CollectResultSets<T>(int resultCount, string script, Func<IDataRecord, T> map, object parameters = null)
        {
            return new ScriptDbQuery(script, parameters).CollectResultSets(resultCount, map);
        }

        public static List<T>[] CollectResultSets<T>(DbSessionBase session, int resultCount, string script, Func<IDataRecord, T> map, object parameters = null)
        {
            return new ScriptDbQuery(script, parameters).CollectResultSets(resultCount, map);
        }


        public static List<IDataRecord>[] CollectResultSets(int resultCount, string script, object parameters = null)
        {
            return new ScriptDbQuery(script, parameters).CollectResultSets(resultCount);
        }

        public static List<IDataRecord>[] CollectResultSets(DbSessionBase session, int resultCount, string script, object parameters = null)
        {
            return new ScriptDbQuery(script, parameters).CollectResultSets(resultCount);
        }


        public static List<Dictionary<string, object>>[] CollectResultSetsAsDictionaries(int resultCount, string script, object parameters = null)
        {
            return new ScriptDbQuery(script, parameters).CollectResultSetsAsDictionaries(resultCount);
        }

        public static List<Dictionary<string, object>>[] CollectResultSetsAsDictionaries(DbSessionBase session, int resultCount, string script, object parameters = null)
        {
            return new ScriptDbQuery(script, parameters).CollectResultSetsAsDictionaries(resultCount);
        }

        #endregion


        // TODO: add all versions that take DbConnection or DbTransaction

        #region --- Async Queries ---

        public static async Task<T> GetScalarAsync<T>(string script, object parameters = null)
        {
            return await new ScriptDbQuery(script, parameters).GetScalarAsync<T>();
        }

        public static async Task<T> GetScalarAsync<T>(DbSessionBase session, string script, object parameters = null)
        {
            return await new ScriptDbQuery(session, script, parameters).GetScalarAsync<T>();
        }

        public static async Task<T> GetScalarAsync<T>(string script, T dbNullSubstitute, object parameters = null)
        {
            return await new ScriptDbQuery(script, parameters).GetScalarAsync(dbNullSubstitute);
        }

        public static async Task<T> GetScalarAsync<T>(DbSessionBase session, T dbNullSubstitute, string script, object parameters = null)
        {
            return await new ScriptDbQuery(session, script, parameters).GetScalarAsync(dbNullSubstitute);
        }



        public static async Task<IDataRecord> GetRecordAsync(string script, object parameters = null)
        {
            return await new ScriptDbQuery(script, parameters).GetRecordAsync();
        }

        public static async Task<IDataRecord> GetRecordAsync(DbSessionBase session, string script, object parameters = null)
        {
            return await new ScriptDbQuery(session, script, parameters).GetRecordAsync();
        }


        public static async Task<DataTable> GetDataTableAsync(string script, string tableName, object parameters = null)
        {
            return await new ScriptDbQuery(script, parameters).GetDataTableAsync(tableName);
        }

        public static async Task<DataTable> GetDataTableAsync(DbSessionBase session, string script, string tableName, object parameters = null)
        {
            return await new ScriptDbQuery(session, script, parameters).GetDataTableAsync(tableName);
        }


        public static async Task<DataSet> GetDataSetAsync(string script, string[] tableNames, object parameters = null)
        {
            return await new ScriptDbQuery(script, parameters).GetDataSetAsync(tableNames);
        }
        
        public static async Task<DataSet> GetDataSetAsync(DbSessionBase session, string script, string[] tableNames, object parameters = null)
        {
            return await new ScriptDbQuery(session, script, parameters).GetDataSetAsync(tableNames);
        }


        public static async Task ProcessResultSetAsync(string script, Action<IDataRecord> process, object parameters = null)
        {
            await new ScriptDbQuery(script, parameters).ProcessResultSetAsync(process);
        }

        public static async Task ProcessResultSetAsync(DbSessionBase session, string script, Action<IDataRecord> process, object parameters = null)
        {
            await new ScriptDbQuery(session, script, parameters).ProcessResultSetAsync(process);
        }


        public static async Task<List<T>> CollectResultSetAsync<T>(string script, Func<IDataRecord, T> map, object parameters = null)
        {
            return await new ScriptDbQuery(script, parameters).CollectResultSetAsync(map);
        }

        public static async Task<List<T>> CollectResultSetAsync<T>(DbSessionBase session, string script, Func<IDataRecord, T> map, object parameters = null)
        {
            return await new ScriptDbQuery(session, script, parameters).CollectResultSetAsync(map);
        }


        public static async Task<List<IDataRecord>> CollectResultSetAsync(string script, object parameters = null)
        {
            return await new ScriptDbQuery(script, parameters).CollectResultSetAsync();
        }

        public static async Task<List<IDataRecord>> CollectResultSetAsync(DbSessionBase session, string script, object parameters = null)
        {
            return await new ScriptDbQuery(session, script, parameters).CollectResultSetAsync();
        }


        public static async Task<List<Dictionary<string, object>>> CollectResultSetAsDictionariesAsync(string script, object parameters = null)
        {
            return await new ScriptDbQuery(script, parameters).CollectResultSetAsDictionariesAsync();
        }

        public static async Task<List<Dictionary<string, object>>> CollectResultSetAsDictionariesAsync(DbSessionBase session, string script, object parameters = null)
        {
            return await new ScriptDbQuery(session, script, parameters).CollectResultSetAsDictionariesAsync();
        }



        public static async Task ProcessResultSetsAsync(string script, params Action<IDataRecord>[] processes)
        {
            await new ScriptDbQuery(script).ProcessResultSetsAsync(processes);
        }

        public static async Task ProcessResultSetsAsync(DbSessionBase session, string script, params Action<IDataRecord>[] processes)
        {
            await new ScriptDbQuery(session, script).ProcessResultSetsAsync(processes);
        }

        public static async Task ProcessResultSetsAsync(string script, object parameters, params Action<IDataRecord>[] processes)
        {
            await new ScriptDbQuery(script, parameters).ProcessResultSetsAsync(processes);
        }

        public static async Task ProcessResultSetsAsync(DbSessionBase session, string script, object parameters, params Action<IDataRecord>[] processes)
        {
            await new ScriptDbQuery(session, script, parameters).ProcessResultSetsAsync(processes);
        }


        public static async Task<List<T>[]> CollectResultSetsAsync<T>(int resultCount, Func<IDataRecord, T> map, string script, object parameters = null)
        {
            return await new ScriptDbQuery(script, parameters).CollectResultSetsAsync(resultCount, map);
        }

        public static async Task<List<T>[]> CollectResultSetsAsync<T>(DbSessionBase session, int resultCount, Func<IDataRecord, T> map, string script, object parameters = null)
        {
            return await new ScriptDbQuery(session, script, parameters).CollectResultSetsAsync(resultCount, map);
        }


        public static async Task<List<IDataRecord>[]> CollectResultSetsAsync(int resultCount, string script, object parameters = null)
        {
            return await new ScriptDbQuery(script, parameters).CollectResultSetsAsync(resultCount);
        }
        
        public static async Task<List<IDataRecord>[]> CollectResultSetsAsync(DbSessionBase session, int resultCount, string script, object parameters = null)
        {
            return await new ScriptDbQuery(session, script, parameters).CollectResultSetsAsync(resultCount);
        }


        public static async Task<List<Dictionary<string, object>>[]> CollectResultSetsAsDictionariesAsync(int resultCount, string script, object parameters = null)
        {
            return await new ScriptDbQuery(script, parameters).CollectResultSetsAsDictionariesAsync(resultCount);
        }
        
        public static async Task<List<Dictionary<string, object>>[]> CollectResultSetsAsDictionariesAsync(DbSessionBase session, int resultCount, string script, object parameters = null)
        {
            return await new ScriptDbQuery(session, script, parameters).CollectResultSetsAsDictionariesAsync(resultCount);
        }

        #endregion


        #region --- Synchronous Commands ---

        public static void Execute(string script, object parameters = null)
        {
            new ScriptDbCommand(script, parameters).Execute();
        }

        public static void Execute(IsolationLevel isolationLevel, string script, object parameters = null)
        {
            new ScriptDbCommand(script, parameters).Execute(isolationLevel);
        }

        public static void Execute(DbSessionBase session, string script, object parameters = null)
        {
            new ScriptDbCommand(session, script, parameters).Execute();
        }

        public static void Execute(DbConnection connection, string script, object parameters = null)
        {
            new ScriptDbCommand(connection, script, parameters).Execute();
        }

        public static void Execute(DbTransaction transaction, string script, object parameters = null)
        {
            new ScriptDbCommand(transaction, script, parameters).Execute();
        }

        public static void ExecuteWithoutTransaction(string script, object parameters = null)
        {
            new ScriptDbCommand(script, parameters).ExecuteWithoutTransaction();
        }

        public static void ExecuteWithoutTransaction(DbConnection connection, string script, object parameters = null)
        {
            new ScriptDbCommand(connection, script, parameters).ExecuteWithoutTransaction();
        }

        #endregion


        #region --- Async Commands ---

        public static async Task ExecuteAsync(string script, object parameters = null)
        {
            await new ScriptDbCommand(script, parameters).ExecuteAsync();
        }

        public static async Task ExecuteAsync(IsolationLevel isolationLevel, string script, object parameters = null)
        {
            await new ScriptDbCommand(script, parameters).ExecuteAsync(isolationLevel);
        }

        public static async Task ExecuteAsync(DbSessionBase session, string script, object parameters = null)
        {
            await new ScriptDbCommand(session, script, parameters).ExecuteAsync();
        }

        public static async Task ExecuteAsync(DbConnection connection, string script, object parameters = null)
        {
            await new ScriptDbCommand(connection, script, parameters).ExecuteAsync();
        }

        public static async Task ExecuteAsync(DbTransaction transaction, string script, object parameters = null)
        {
            await new ScriptDbCommand(transaction, script, parameters).ExecuteAsync();
        }

        public static async Task ExecuteWithoutTransactionAsync(string script, object parameters = null)
        {
            await new ScriptDbCommand(script, parameters).ExecuteWithoutTransactionAsync();
        }
        
        public static async Task ExecuteWithoutTransactionAsync(DbConnection connection, string script, object parameters = null)
        {
            await new ScriptDbCommand(connection, script, parameters).ExecuteWithoutTransactionAsync();
        }

        #endregion
    }
}
