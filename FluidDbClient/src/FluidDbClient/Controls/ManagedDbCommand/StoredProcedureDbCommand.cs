
using System.Data;


namespace FluidDbClient
{
    public abstract class StoredProcedureDbCommandBase : ManagedDbCommand
    {
        protected StoredProcedureDbCommandBase(Database database, DbSessionBase session, string storedProcedureName, object parameters)
            : base(database, session, parameters)
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
        public StoredProcedureDbCommand(DbSessionBase session, string storedProcedureName, object parameters = null) 
            : base(DbRegistry.GetDatabase(), session, storedProcedureName, parameters)
        { }

        public StoredProcedureDbCommand(string storedProcedureName, object parameters = null)
            : this(null, storedProcedureName, parameters)
        { }
    }


    public class StoredProcedureDbCommand<TDatabase> : StoredProcedureDbCommandBase where TDatabase : Database
    {
        public StoredProcedureDbCommand(DbSessionBase session, string storedProcedureName, object parameters = null)
                    : base(DbRegistry.GetDatabase<TDatabase>(), session, storedProcedureName, parameters)
        { }

        public StoredProcedureDbCommand(string storedProcedureName, object parameters = null)
            : this(null, storedProcedureName, parameters)
        { }
    }
}
