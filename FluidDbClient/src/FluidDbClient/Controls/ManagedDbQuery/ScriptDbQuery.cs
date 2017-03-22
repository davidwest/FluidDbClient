using System;
using System.Data;

namespace FluidDbClient
{
    public abstract class ScriptDbQueryBase : ManagedDbQuery
    {
        private readonly string _staticScript;
        private string _includedScript;

        protected ScriptDbQueryBase(Database database, DbSessionBase session, string staticScript, object parameters) 
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


        protected override string CommandText => Script;

        protected override CommandType CommandType => CommandType.Text;
    }


    public class ScriptDbQuery : ScriptDbQueryBase
    {
        public ScriptDbQuery(DbSessionBase session, string staticScript, object parameters = null) 
            : base(DbRegistry.GetDatabase(), session, staticScript, parameters)
        { }

        public ScriptDbQuery(string staticScript, object parameters = null) 
            : this(null, staticScript, parameters)
        { }

        public ScriptDbQuery() : this(null, null)
        { }
    }


    public class ScriptDbQuery<TDatabase> : ScriptDbQueryBase where TDatabase : Database
    {
        public ScriptDbQuery(DbSessionBase session, string staticScript, object parameters = null)
            : base(DbRegistry.GetDatabase<TDatabase>(), session, staticScript, parameters)
        { }

        public ScriptDbQuery(string staticScript, object parameters = null)
            : this(null, staticScript, parameters)
        { }

        public ScriptDbQuery() : this(null, null)
        { }
    }
}
