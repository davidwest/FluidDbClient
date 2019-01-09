using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FluidDbClient.Sql
{
    public class SqlBulkInserter
    {
        private SqlBulkCopyOptions _options = SqlBulkCopyOptions.Default;
        private string _tableName = string.Empty;
        private int _batchSize;
        private int _timeout;
        private int _notifyAfter;
        private SqlRowsCopiedEventHandler[] _copiedEventHandlers = new SqlRowsCopiedEventHandler[0];

        private DataTable _dataTable = new DataTable();
        private IDataReader _dataReader;

        private string _connectionString = DbRegistry.GetDatabase()?.ConnectionString;
        private SqlConnection _connection;
        private SqlTransaction _transaction;
        
        public SqlBulkInserter GoesToTable(string tableName)
        {
            _tableName = tableName;
            return this;
        }
        
        public SqlBulkInserter FromSource(DataTable dataTable)
        {
            _dataTable = dataTable;
            _dataReader = null;
            return this;
        }
        
        public SqlBulkInserter FromSource(IDataReader reader)
        {
            _dataReader = reader;
            _dataTable = null;
            
            return this;
        }

        public SqlBulkInserter FromSource<T>(IEnumerable<T> items) where T : class
        {
            return FromSource(items.AsDataReader());
        }

        public SqlBulkInserter HasOptions(SqlBulkCopyOptions options)
        {
            _options = options;
            return this;
        }
        
        public SqlBulkInserter HasBatchSize(int batchSize)
        {
            _batchSize = batchSize >= 0 ? batchSize : 0;
            return this;
        }
        
        public SqlBulkInserter TimeoutAfter(int timeoutInSeconds)
        {
            _timeout = timeoutInSeconds >= 0 ? timeoutInSeconds : 0;
            return this;
        }
        
        public SqlBulkInserter HasNotifications(int notifyAfter, params SqlRowsCopiedEventHandler[] eventHandlers)
        {
            _notifyAfter = notifyAfter >= 0 ? notifyAfter : 0;
            _copiedEventHandlers = eventHandlers;
            return this;
        }
        
        public SqlBulkInserter UseConnection(SqlConnection connection)
        {
            _connectionString = null;
            _connection = connection;
            return this;
        }

        public SqlBulkInserter UseTransaction(SqlTransaction transaction)
        {
            _options &= ~SqlBulkCopyOptions.UseInternalTransaction;

            _connectionString = null;
            _transaction = transaction;
            _connection = transaction.Connection;
            return this;
        }

        public SqlBulkInserter UseInternalResources()
        {
            _connectionString = DbRegistry.GetDatabase()?.ConnectionString;
            _connection = null;
            _transaction = null;
            return this;
        }

        public SqlBulkInserter UseInternalResources<TDatabase>() where TDatabase : Database
        {
            _connectionString = DbRegistry.GetDatabase<TDatabase>()?.ConnectionString;
            _connection = null;
            _transaction = null;
            return this;
        }

        public void Write()
        {
            if (_connection != null && !_connection.State.HasFlag(ConnectionState.Open))
            {
                _connection.Open();
            }

            using (var copier = CreateSqlBulkCopy())
            {
                if (_dataReader != null)
                {
                    copier.WriteToServer(_dataReader);
                }
                else
                {
                    copier.WriteToServer(_dataTable);
                }
            }
        }

        public async Task WriteAsync()
        {
            if (_connection != null && !_connection.State.HasFlag(ConnectionState.Open))
            {
                await _connection.OpenAsync();
            }

            using (var copier = CreateSqlBulkCopy())
            {
                if (_dataReader != null)
                {
                    await copier.WriteToServerAsync(_dataReader);
                }
                else
                {
                    await copier.WriteToServerAsync(_dataTable);
                }
            }
        }
        
        private SqlBulkCopy CreateSqlBulkCopy()
        {
            SqlBulkCopy copier;

            if (_transaction != null)
            {
                copier = new SqlBulkCopy(_connection, _options, _transaction);
            }
            else if (_connection != null)
            {
                copier = new SqlBulkCopy(_connection, _options, null);
            }
            else
            {
                copier = new SqlBulkCopy(_connectionString, _options);
            }
            
            foreach (var handlers in _copiedEventHandlers)
            {
                copier.SqlRowsCopied += handlers;
            }

            copier.DestinationTableName = _tableName;
            copier.BatchSize = _batchSize;
            copier.BulkCopyTimeout = _timeout;
            copier.NotifyAfter = _notifyAfter;

            return copier;
        }
    }
}
