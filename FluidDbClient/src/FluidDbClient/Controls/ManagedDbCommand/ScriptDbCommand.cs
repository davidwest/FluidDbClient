using System;
using System.Data;
using System.Data.Common;

namespace FluidDbClient
{
    public abstract class ScriptDbCommandBase : ManagedDbCommand
    {
        private readonly string _staticScript;
        private string _includedScript;

        protected ScriptDbCommandBase(Database database, string staticScript, object parameters) 
            : base(database, parameters)
        {
            _staticScript = staticScript ?? string.Empty;
        }

        protected ScriptDbCommandBase(Database database, DbSessionBase session, string staticScript, object parameters) 
            : base(database, session, parameters)
        {
            _staticScript = staticScript ?? string.Empty;
        }

        protected ScriptDbCommandBase(Database database, DbConnection connection, string staticScript, object parameters) 
            : base(database, connection, parameters)
        {
            _staticScript = staticScript ?? string.Empty;
        }

        protected ScriptDbCommandBase(Database database, DbTransaction transaction, string staticScript, object parameters)
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
        
        protected override CommandType CommandType => CommandType.Text;
        protected override string CommandText => Script;
    }


    public class ScriptDbCommand : ScriptDbCommandBase
    {
        public ScriptDbCommand(string staticScript, object parameters = null)
            : base(DbRegistry.GetDatabase(), staticScript, parameters)
        { }

        public ScriptDbCommand(DbSessionBase session, string staticScript, object parameters = null) 
            : base(DbRegistry.GetDatabase(), session, staticScript, parameters)
        { }
        
        public ScriptDbCommand(DbConnection connection, string staticScript, object parameters = null) 
            : base(DbRegistry.GetDatabase(), connection, staticScript, parameters)
        { }

        public ScriptDbCommand(DbTransaction transaction, string staticScript, object parameters = null)
            : base(DbRegistry.GetDatabase(), transaction, staticScript, parameters)
        { }

        public ScriptDbCommand()
            : base(DbRegistry.GetDatabase(), null, null)
        { }

        public ScriptDbCommand(DbSessionBase session) : this(session, null)
        { }

        public ScriptDbCommand(DbConnection connection) : this(connection, null)
        { }

        public ScriptDbCommand(DbTransaction transaction) : this(transaction, null)
        { }
    }


    public class ScriptDbCommand<TDatabase> : ScriptDbCommandBase where TDatabase : Database
    {
        public ScriptDbCommand(string staticScript, object parameters = null)
            : base(DbRegistry.GetDatabase<TDatabase>(), staticScript, parameters)
        { }

        public ScriptDbCommand(DbSessionBase session, string staticScript, object parameters = null)
            : base(DbRegistry.GetDatabase<TDatabase>(), session, staticScript, parameters)
        { }

        public ScriptDbCommand(DbConnection connection, string staticScript, object parameters = null)
            : base(DbRegistry.GetDatabase<TDatabase>(), connection, staticScript, parameters)
        { }

        public ScriptDbCommand(DbTransaction transaction, string staticScript, object parameters = null)
            : base(DbRegistry.GetDatabase<TDatabase>(), transaction, staticScript, parameters)
        { }

        public ScriptDbCommand()
            : base(DbRegistry.GetDatabase<TDatabase>(), null, null)
        { }

        public ScriptDbCommand(DbSessionBase session) : this(session, null)
        { }

        public ScriptDbCommand(DbConnection connection) : this(connection, null)
        { }

        public ScriptDbCommand(DbTransaction transaction) : this(transaction, null)
        { }
    }
}
