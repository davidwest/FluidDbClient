using System.Data;
using System.Data.Common;

namespace FluidDbClient
{
    public abstract class StoredProcedureDbQueryBase : ManagedDbQuery
    {
        protected StoredProcedureDbQueryBase(Database database, string storedProcedureName, object parameters) 
            : base(database, parameters)
        {
            StoredProcedureName = storedProcedureName;
        }

        protected StoredProcedureDbQueryBase(Database database, DbSessionBase session, string storedProcedureName, object parameters) 
            : base(database, session, parameters)
        {
            StoredProcedureName = storedProcedureName;
        }

        protected StoredProcedureDbQueryBase(Database database, DbConnection connection, string storedProcedureName, object parameters)
            : base(database, connection, parameters)
        {
            StoredProcedureName = storedProcedureName;
        }

        protected StoredProcedureDbQueryBase(Database database, DbTransaction transaction, string storedProcedureName, object parameters)
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


    public class StoredProcedureDbQuery : StoredProcedureDbQueryBase
    {
        public StoredProcedureDbQuery(string storedProcedureName, object parameters = null) 
            : base(DbRegistry.GetDatabase(), storedProcedureName, parameters)
        { }

        public StoredProcedureDbQuery(DbSessionBase session, string storedProcedureName, object parameters = null) 
            : base(DbRegistry.GetDatabase(), session, storedProcedureName, parameters)
        { }

        public StoredProcedureDbQuery(DbConnection connection, string storedProcedureName, object parameters = null)
            : base(DbRegistry.GetDatabase(), connection, storedProcedureName, parameters)
        { }

        public StoredProcedureDbQuery(DbTransaction transaction, string storedProcedureName, object parameters = null)
            : base(DbRegistry.GetDatabase(), transaction, storedProcedureName, parameters)
        { }
    }

    public class StoredProcedureDbQuery<TDatabase> : StoredProcedureDbQueryBase where TDatabase : Database
    {
        public StoredProcedureDbQuery(string storedProcedureName, object parameters = null)
            : base(DbRegistry.GetDatabase<TDatabase>(), storedProcedureName, parameters)
        { }

        public StoredProcedureDbQuery(DbSessionBase session, string storedProcedureName, object parameters = null)
            : base(DbRegistry.GetDatabase<TDatabase>(), session, storedProcedureName, parameters)
        { }

        public StoredProcedureDbQuery(DbConnection connection, string storedProcedureName, object parameters = null)
            : base(DbRegistry.GetDatabase<TDatabase>(), connection, storedProcedureName, parameters)
        { }

        public StoredProcedureDbQuery(DbTransaction transaction, string storedProcedureName, object parameters = null)
            : base(DbRegistry.GetDatabase<TDatabase>(), transaction, storedProcedureName, parameters)
        { }
    }
}
