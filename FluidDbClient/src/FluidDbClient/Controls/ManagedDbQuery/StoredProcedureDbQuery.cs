using System.Data;

namespace FluidDbClient
{
    // TODO: more constructor options

    public abstract class StoredProcedureDbQueryBase : ManagedDbQuery
    {
        protected StoredProcedureDbQueryBase(Database database, DbSessionBase session, string storedProcedureName, object parameters) 
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


    public class StoredProcedureDbQuery : StoredProcedureDbQueryBase
    {
        public StoredProcedureDbQuery(DbSessionBase session, string storedProcedureName, object parameters = null) 
            : base(DbRegistry.GetDatabase(), session, storedProcedureName, parameters)
        { }

        public StoredProcedureDbQuery(string storedProcedureName, object parameters = null) 
            : this(null, storedProcedureName, parameters)
        { }
    }

    public class StoredProcedureDbQuery<TDatabase> : StoredProcedureDbQueryBase where TDatabase : Database
    {
        public StoredProcedureDbQuery(DbSessionBase session, string storedProcedureName, object parameters = null)
            : base(DbRegistry.GetDatabase<TDatabase>(), session, storedProcedureName, parameters)
        { }

        public StoredProcedureDbQuery(string storedProcedureName, object parameters = null)
            : this(null, storedProcedureName, parameters)
        { }
    }
}
