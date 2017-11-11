using System.Data;
using System.Data.Common;

namespace FluidDbClient
{
    public abstract partial class ManagedDbCommand : ManagedDbControl, IManagedDbCommand
    {
        private bool _usingExternalResources;
        private bool _isCommitted;
        
        protected ManagedDbCommand(Database database, object parameters) 
            : base(database, parameters)
        { }

        protected ManagedDbCommand(Database database, DbSessionBase session, object parameters)
            : base(database, session, parameters)
        {
            _usingExternalResources = session != null;
        }

        protected ManagedDbCommand(Database database, DbConnection connection, object parameters)
            : base(database, connection, parameters)
        {
            _usingExternalResources = connection != null;
        }

        protected ManagedDbCommand(Database database, DbTransaction transaction, object parameters)
            : base(database, transaction, parameters)
        {
            _usingExternalResources = transaction != null;
        }

        public void Execute(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            try
            {
                OnOperationStarted();

                EstablishResources(isolationLevel);

                Command.ExecuteNonQuery();

                TryCommit();
            }
            finally
            {
                ReleaseResources();
            }
        }
        
        private void EstablishResources(IsolationLevel isolationLevel)
        {            
            EstablishConnection();
            EstablishTransaction(isolationLevel);
            CreateCommand();
        }
        
        private void ReleaseResources()
        {
            DisposeCommand();
            ReleaseTransaction();
            ReleaseConnection(_usingExternalResources);
        }

        private void EstablishTransaction(IsolationLevel isolationLevel)
        {
            if (Transaction != null || _usingExternalResources) return;
                
            Transaction = Connection.BeginTransaction(isolationLevel);

            Log("Created DbTransaction");
        }

        private void TryCommit()
        {
            if (_usingExternalResources || Transaction == null) return;

            Transaction.Commit();
            _isCommitted = true;

            Log($"Committed DbTransaction {(char)0x221A}");
        }

        private void ReleaseTransaction()
        {
            if (Transaction == null) return;

            if (_usingExternalResources)
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
