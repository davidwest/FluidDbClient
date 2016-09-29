
using System.Data;

namespace FluidDbClient
{
    public abstract partial class ManagedDbCommand : ManagedDbControl, IManagedDbCommand
    {
        private readonly bool _usingExternalSession;
        private bool _isCommitted;

        protected ManagedDbCommand(Database database, DbSessionBase session, object parameters)
            : base(database, session, parameters)
        {
            _usingExternalSession = session != null;
        }

        public void Execute(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {            
            try
            {
                CreateResources(isolationLevel);

                Command.ExecuteNonQuery();

                Commit();
            }
            finally
            {
                DisposeResources();
            }
        }

        private void CreateResources(IsolationLevel isolationLevel)
        {
            OnOperationStarted();

            CreateConnection();
            CreateTransaction(isolationLevel);
            CreateCommand();
        }

        private void DisposeResources()
        {
            DisposeCommand();

            DisposeTransaction();

            ReleaseConnection(_usingExternalSession);
        }

        private void CreateTransaction(IsolationLevel isolationLevel)
        {
            if (Transaction != null) return;

            if (!Connection.State.HasFlag(ConnectionState.Open))
            {
                Connection.Open();
                Log("Opened DbConnection");
            }
                
            Transaction = Connection.BeginTransaction(isolationLevel);

            Log("Created DbTransaction");
        }

        private void Commit()
        {
            if (_usingExternalSession || Transaction == null) return;

            Transaction.Commit();
            _isCommitted = true;

            Log($"Committed DbTransaction {(char)0x221A}");
        }

        private void DisposeTransaction()
        {
            if (Transaction == null) return;

            if (_usingExternalSession)
            {
                Transaction = null;
                return;
            }

            Transaction.Dispose();
            Transaction = null;

            Log($"Disposed DbTransaction {(_isCommitted ? "" : "(Changes rolled back)")}");

            _isCommitted = false;
        }
    }
}
