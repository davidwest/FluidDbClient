
using System;
using System.Data;

namespace FluidDbClient
{
    public abstract class ScriptDbCommandBase : ManagedDbCommand
    {
        private readonly string _staticScript;
        private string _includedScript;

        protected ScriptDbCommandBase(Database database, DbSessionBase session, string staticScript, object parameters) 
            : base(database, session, parameters)
        {
            _staticScript = staticScript ?? string.Empty;
        }

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

        public string Script => $"{_staticScript}\n{_includedScript}";

        protected override CommandType CommandType => CommandType.Text;
        protected override string CommandText => Script;
    }


    public class ScriptDbCommand : ScriptDbCommandBase
    {
        public ScriptDbCommand(DbSessionBase session, string staticScript, object parameters = null) 
            : base(DbRegistry.GetDatabase(), session, staticScript, parameters)
        { }

        public ScriptDbCommand(string staticScript, object parameters = null) 
            : this(null, staticScript, parameters)
        { }

        public ScriptDbCommand() : this(null, null)
        { }
    }


    public class ScriptDbCommand<TDatabase> : ScriptDbCommandBase where TDatabase : Database
    {
        public ScriptDbCommand(DbSessionBase session, string staticScript, object parameters = null)
            : base(DbRegistry.GetDatabase<TDatabase>(), session, staticScript, parameters)
        { }

        public ScriptDbCommand(string staticScript, object parameters = null)
            : this(null, staticScript, parameters)
        { }

        public ScriptDbCommand() : this(null, null)
        { }
    }
}
