
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluidDbClient.Extensions;

namespace FluidDbClient
{
    public abstract class ManagedDbControl : IManagedDbControl
    {
        private readonly Database _database;
        private readonly DbSessionBase _session;
        private readonly string _typeName;
        private readonly Dictionary<string, DbParameter> _parameters = new Dictionary<string, DbParameter>(StringComparer.OrdinalIgnoreCase);

        protected ManagedDbControl(Database database, DbSessionBase session, object parameters)
        {
            Timeout = 30;

            _database = database;
            _session = session;
            _typeName = GetType().Name;

            if (parameters == null) return;

            var mappedParams = parameters.GetPropertyMap();

            mappedParams.ForEach(p => this[p.Key] = p.Value);
        }

        public int Timeout { get; set; }

        public IReadOnlyCollection<DbParameter> Parameters => _parameters.Values.ToArray();
        
        public object this[string parameterName]
        {
            get
            {
                DbParameter parameter;
                _parameters.TryGetValue(parameterName, out parameter);
                
                if (parameter == null)
                {
                    _parameters.TryGetValue(GetUnprefixedParameterName(parameterName), out parameter);
                }

                return parameter?.Value;
            }
            set
            {
                var param = _database.Provider.CreateParameter(parameterName, value);
                _parameters[parameterName] = param;
            }
        }

        public void AddParameters(params DbParameter[] parameters)
        {
            AddParameters(parameters.AsEnumerable());
        }

        public void AddParameters(IEnumerable<DbParameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                var unprefixedName = GetUnprefixedParameterName(parameter.ParameterName);

                _parameters.Add(unprefixedName, parameter);
            }
        }
        
        public virtual void ClearSpecificationState()
        {
            _parameters.Clear();
        }

        public virtual string ToDiagnosticString()
        {
            if (_parameters.Count == 0) return string.Empty;

            var builder = new StringBuilder("--- Parameters ---\n");

            _parameters.Values.ForEach(p => builder.AppendLine(_database.Provider.TextInterpreter.GetDiagnosticString(p)));

            return builder.ToString();
        }
        
        protected abstract CommandType CommandType { get; }
        protected abstract string CommandText { get; }

        public Guid SessionId { get; private set; }
        protected Guid OperationId { get; private set; }

        protected DbConnection Connection { get; private set; }
        protected DbTransaction Transaction { get; set; }
        protected DbCommand Command { get; private set; }
        

        protected void CreateConnection()
        {
            if (_session != null)
            {
                Transaction = _session.GetTransaction();
                Connection = Transaction.Connection;
                return;
            }

            Connection = _database.Provider.CreateConnection(_database.ConnectionString);

            Log("Created DbConnection");
        }

        protected async Task CreateConnectionAsync()
        {
            if (_session != null)
            {
                Transaction = await _session.GetTransactionAsync();
                Connection = Transaction.Connection;
                return;
            }

            Connection = _database.Provider.CreateConnection(_database.ConnectionString);

            Log("Created DbConnection");
        }

        protected void CreateCommand()
        {
            var command = Transaction != null
                ? _database.Provider.CreateCommandFrom(Transaction)
                : _database.Provider.CreateCommandFrom(Connection);

            command.CommandText = CommandText;
            command.CommandTimeout = Timeout;
            command.CommandType = CommandType;

            foreach (var param in Parameters)
            {
                command.Parameters.Add(param);
            }

            Command = command;

            Log("Created DbCommand");
        }
        
        protected void ReleaseConnection(bool usingExternalSession)
        {
            if (Connection == null) return;

            if (usingExternalSession)
            {
                Connection = null;
                return;
            }

            Connection.Dispose();
            Connection = null;

            Log("Disposed DbConnection");
        }

        protected void DisposeCommand()
        {
            if (Command == null) return;

            Command.Dispose();
            Command = null;

            Log("Disposed DbCommand");
        }

        protected void Log(string message)
        {
            _database.Log($"{SessionId} : {_typeName, -30} : {OperationId} : {message}");
        }

        
        protected void OnOperationStarted()
        {
            SessionId = _session?.SessionId ?? Guid.NewGuid();
            OperationId = Guid.NewGuid();
        }

        private string GetUnprefixedParameterName(string parameterName)
        {
            return _database.Provider.TextInterpreter.GetUnprefixedParameterName(parameterName);
        }
    }
}
