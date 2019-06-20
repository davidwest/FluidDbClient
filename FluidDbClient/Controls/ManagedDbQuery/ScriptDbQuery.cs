using System;
using System.Data;
using System.Data.Common;

namespace FluidDbClient
{
    public abstract class ScriptDbQueryBase : ManagedDbQuery
    {
        private readonly string _staticScript;
        private string _includedScript;

        protected ScriptDbQueryBase(Database database, string staticScript, object parameters) 
            : base(database, parameters)
        {
            _staticScript = staticScript ?? string.Empty;
        }

        protected ScriptDbQueryBase(Database database, DbSessionBase session, string staticScript, object parameters) 
            : base(database, session, parameters)
        {
            _staticScript = staticScript ?? string.Empty;
        }

        protected ScriptDbQueryBase(Database database, DbConnection connection, string staticScript, object parameters)
            : base(database, connection, parameters)
        {
            _staticScript = staticScript ?? string.Empty;
        }

        protected ScriptDbQueryBase(Database database, DbTransaction transaction, string staticScript, object parameters)
            : base(database, transaction, parameters)
        {
            _staticScript = staticScript ?? string.Empty;
        }

        public string Script => $"{ReplaceMultiParametersIn(_staticScript)}\n{_includedScript}";

        public void IncludeScriptDoc(DbScriptDocument doc)
        {
            if (_includedScript != null)
            {
                throw new InvalidOperationException("A DbScriptDocument has already been included");
            }

            _includedScript = doc.Text;
            AddParameters(doc.Parameters);
        }

        public override void ClearSpecificationState()
        {
            _includedScript = null;
            base.ClearSpecificationState();
        }

        public override string ToDiagnosticString()
        {
            return $"\n{Script}\n" + base.ToDiagnosticString();
        }
        
        protected override string CommandText => Script;

        protected override CommandType CommandType => CommandType.Text;
    }


    public class ScriptDbQuery : ScriptDbQueryBase
    {
        public ScriptDbQuery(string staticScript, object parameters = null) 
            : base(DbRegistry.GetDatabase(), staticScript, parameters)
        { }

        public ScriptDbQuery(DbSessionBase session, string staticScript, object parameters = null) 
            : base(DbRegistry.GetDatabase(), session, staticScript, parameters)
        { }

        public ScriptDbQuery(DbConnection connection, string staticScript, object parameters = null)
            : base(DbRegistry.GetDatabase(), connection, staticScript, parameters)
        { }

        public ScriptDbQuery(DbTransaction transaction, string staticScript, object parameters = null)
            : base(DbRegistry.GetDatabase(), transaction, staticScript, parameters)
        { }

        public ScriptDbQuery() 
            : base(DbRegistry.GetDatabase(), null, null)
        { }

        public ScriptDbQuery(DbSessionBase session) : this(session, null)
        { }

        public ScriptDbQuery(DbConnection connection) : this(connection, null)
        { }
        
        public ScriptDbQuery(DbTransaction transaction) : this(transaction, null)
        { }
    }


    public class ScriptDbQuery<TDatabase> : ScriptDbQueryBase where TDatabase : Database
    {
        public ScriptDbQuery(string staticScript, object parameters = null)
            : base(DbRegistry.GetDatabase<TDatabase>(), staticScript, parameters)
        { }

        public ScriptDbQuery(DbSessionBase session, string staticScript, object parameters = null)
            : base(DbRegistry.GetDatabase<TDatabase>(), session, staticScript, parameters)
        { }

        public ScriptDbQuery(DbConnection connection, string staticScript, object parameters = null)
            : base(DbRegistry.GetDatabase<TDatabase>(), connection, staticScript, parameters)
        { }

        public ScriptDbQuery(DbTransaction transaction, string staticScript, object parameters = null)
            : base(DbRegistry.GetDatabase<TDatabase>(), transaction, staticScript, parameters)
        { }

        public ScriptDbQuery()
            : base(DbRegistry.GetDatabase<TDatabase>(), null, null)
        { }

        public ScriptDbQuery(DbSessionBase session) : this(session, null)
        { }

        public ScriptDbQuery(DbConnection connection) : this(connection, null)
        { }

        public ScriptDbQuery(DbTransaction transaction) : this(transaction, null)
        { }
    }
}
