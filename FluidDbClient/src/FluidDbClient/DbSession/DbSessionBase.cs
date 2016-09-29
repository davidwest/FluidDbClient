
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace FluidDbClient
{
    public abstract class DbSessionBase : IDisposable
    {
        private readonly Database _database;
        private readonly IsolationLevel _isolationLevel;
        private readonly Action<string> _log;
        private readonly string _typeName;

        private DbTransaction _transaction;
        private DbConnection _connection;

        private bool _isRolledBack;
        private bool _isCommitted;

        protected DbSessionBase(Database database, IsolationLevel isolationLevel, Action<string> log)
        {
            SessionId = Guid.NewGuid();

            _database = database;
            _isolationLevel = isolationLevel;
            _typeName = GetType().Name;

            _log = log ?? (msg => { _database.Log(msg); });
        }

        public void Commit()
        {
            if (_isRolledBack)
            {
                throw new InvalidOperationException("A rollback has already occurred: cannot commit");
            }

            if (_transaction == null) return;

            _transaction.Commit();
            _isCommitted = true;

            Log("Session committed");

            Dispose();
        }

        public void RollBack()
        {
            if (_isCommitted)
            {
                throw new InvalidOperationException("A commit has already occured: cannot roll back");
            }

            if (_transaction == null) return;

            _transaction.Rollback();
            _isRolledBack = true;

            Log("Session rolled back");

            Dispose();
        }

        internal Guid SessionId { get; private set; }


        internal DbTransaction GetTransaction()
        {
            if (SessionId == Guid.Empty)
            {
                throw new InvalidOperationException("DbSession cannot be re-used");
            }

            if (_transaction != null)
            {
                return _transaction;
            }

            _connection = _database.Provider.CreateConnection();
                
            Log("Created DbConnection");

            _connection.Open();

            Log("Opened DbConnection");

            _transaction = _connection.BeginTransaction(_isolationLevel);

            Log("Created DbTransaction");

            return _transaction;
        }


        internal async Task<DbTransaction> GetTransactionAsync()
        {
            if (SessionId == Guid.Empty)
            {
                throw new InvalidOperationException("DbSession cannot be re-used");
            }

            if (_transaction != null)
            {
                return _transaction;
            }

            _connection = _database.Provider.CreateConnection();

            Log("Created DbConnection");

            await _connection.OpenAsync().ConfigureAwait(false);

            Log("Opened DbConnection");

            _transaction = _connection.BeginTransaction(_isolationLevel);

            Log("Created DbTransaction");

            return _transaction;
        }


        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;

                Log($"Disposed DbTransaction {(_isCommitted ? string.Empty : "(Changes rolled back)")}");
            }

            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;

                Log("Disposed DbConnection");
            }

            SessionId = Guid.Empty;
        }

        private void Log(string message)
        {
            _log($"{SessionId} : {_typeName,-30} : {message}");
        }
    }
}
