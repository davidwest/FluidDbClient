using System.Data;
using System.Data.Common;

namespace FluidDbClient
{
    public abstract class StoredProcedureDbCommandBase : ManagedDbCommand
    {
        protected StoredProcedureDbCommandBase(Database database, string storedProcedureName, object parameters) 
            : base(database, parameters)
        {
            StoredProcedureName = storedProcedureName;
        }

        protected StoredProcedureDbCommandBase(Database database, DbSessionBase session, string storedProcedureName, object parameters)
            : base(database, session, parameters)
        {
            StoredProcedureName = storedProcedureName;
        }
        
        protected StoredProcedureDbCommandBase(Database database, DbConnection connection, string storedProcedureName, object parameters)
            : base(database, connection, parameters)
        {
            StoredProcedureName = storedProcedureName;
        }

        protected StoredProcedureDbCommandBase(Database database, DbTransaction transaction, string storedProcedureName, object parameters)
            : base(database, transaction, parameters)
        {
            StoredProcedureName = storedProcedureName;
        }

        public string StoredProcedureName { get; }

        public override string ToDiagnosticString()
        {
            return $"\n{StoredProcedureName}\n" + base.ToDiagnosticString();
        }

        protected override CommandType CommandType => CommandType.StoredProcedure;
        protected override string CommandText => StoredProcedureName;
    }


    public class StoredProcedureDbCommand : StoredProcedureDbCommandBase
    {
        public StoredProcedureDbCommand(string storedProcedureName, object parameters = null)
            : base(DbRegistry.GetDatabase(), storedProcedureName, parameters)
        { }

        public StoredProcedureDbCommand(DbSessionBase session, string storedProcedureName, object parameters = null) 
            : base(DbRegistry.GetDatabase(), session, storedProcedureName, parameters)
        { }

        public StoredProcedureDbCommand(DbConnection connection, string storedProcedureName, object parameters = null)
            : base(DbRegistry.GetDatabase(), connection, storedProcedureName, parameters)
        { }

        public StoredProcedureDbCommand(DbTransaction transaction, string storedProcedureName, object parameters = null)
            : base(DbRegistry.GetDatabase(), transaction, storedProcedureName, parameters)
        { }
    }


    public class StoredProcedureDbCommand<TDatabase> : StoredProcedureDbCommandBase where TDatabase : Database
    {
        public StoredProcedureDbCommand(string storedProcedureName, object parameters = null)
            : base(DbRegistry.GetDatabase<TDatabase>(), storedProcedureName, parameters)
        { }

        public StoredProcedureDbCommand(DbSessionBase session, string storedProcedureName, object parameters = null)
            : base(DbRegistry.GetDatabase<TDatabase>(), session, storedProcedureName, parameters)
        { }

        public StoredProcedureDbCommand(DbConnection connection, string storedProcedureName, object parameters = null)
            : base(DbRegistry.GetDatabase<TDatabase>(), connection, storedProcedureName, parameters)
        { }
        
        public StoredProcedureDbCommand(DbTransaction transaction, string storedProcedureName, object parameters = null)
            : base(DbRegistry.GetDatabase<TDatabase>(), transaction, storedProcedureName, parameters)
        { }
    }
}
