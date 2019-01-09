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

        // --- Connection and transaction managed internally ---
        protected ManagedDbControl(Database database, object parameters)
        {
            Timeout = 60;
            
            _database = database;
            _typeName = GetType().Name;

            if (parameters == null) return;

            var mappedParams = parameters.GetPropertyMap(Provider);

            mappedParams.ForEach(p => this[p.Key] = p.Value);
        }

        // --- Connection and transaction managed externally via DbSessionBase ---
        protected ManagedDbControl(Database database, DbSessionBase session, object parameters) 
            : this(database, parameters)
        {
            _session = session;
        }

        // --- Connection managed externally ---
        protected ManagedDbControl(Database database, DbConnection connection, object parameters)
            : this(database, parameters)
        {
            Connection = connection;
        }

        // --- Connection and transaction managed externally ---
        protected ManagedDbControl(Database database, DbTransaction transaction, object parameters)
            : this(database, parameters)
        {
            Transaction = transaction;
            Connection = transaction.Connection;
        }
        
        public int Timeout { get; set; }

        public IReadOnlyCollection<DbParameter> Parameters => _parameters.Values.ToArray();
        
        public object this[string parameterName]
        {
            get
            {
                _parameters.TryGetValue(parameterName, out var parameter);
                
                if (parameter == null)
                {
                    _parameters.TryGetValue(GetUnprefixedParameterName(parameterName), out parameter);
                }

                return parameter?.Value;
            }
            set
            {
                var param = Provider.CreateParameter(parameterName, value);
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

            _parameters.Values.ForEach(p => builder.AppendLine(_database.Provider.Interpreter.GetDiagnosticString(p)));

            return builder.ToString();
        }
        
        protected abstract CommandType CommandType { get; }
        protected abstract string CommandText { get; }

        public Guid SessionId { get; private set; }
        protected Guid OperationId { get; private set; }
        
        protected DbConnection Connection { get; set; }
        protected DbTransaction Transaction { get; set; }
        protected DbCommand Command { get; private set; }

        protected IDbProvider Provider => _database.Provider;

        protected void EstablishConnection()
        {
            if (Connection != null)
            {
                TryOpenConnection();
                return;
            }

            if (_session != null)
            {
                Transaction = _session.GetTransaction();
                Connection = Transaction.Connection;
                return;
            }

            Connection = _database.Provider.CreateConnection();
            Log("Created DbConnection");

            TryOpenConnection();
        }

        protected async Task EstablishConnectionAsync()
        {
            if (Connection != null)
            {
                await TryOpenConnectionAsync();
                return;
            }

            if (_session != null)
            {
                Transaction = await _session.GetTransactionAsync();
                Connection = Transaction.Connection;
                return;
            }

            Connection = _database.Provider.CreateConnection();
            Log("Created DbConnection");

            await TryOpenConnectionAsync();
        }

        protected void CreateCommand()
        {
            var command = Connection.CreateCommand();
            
            if (Transaction != null)
            {
                command.Transaction = Transaction;
            }

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
        
        protected void ReleaseConnection(bool usingExternalResources)
        {
            if (Connection == null) return;

            if (usingExternalResources)
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

        protected string ReplaceMultiParametersIn(string text)
        {
            var multiParamReplacementMap =
                Parameters
                    .Select(p => Provider.Interpreter.GetUnprefixedParameterName(p.ParameterName))
                    .GetMultiParamReplacementMap();

            // TODO: could optimize
            foreach (var grp in multiParamReplacementMap)
            {
                var paramNameToFind = Provider.Interpreter.GetPrefixedParameterName(grp.Key);

                var replacement =
                    grp.Select(indexedName => Provider.Interpreter.GetPrefixedParameterName(indexedName)).ToCsv();

                text = text.Replace(paramNameToFind, replacement);
            }

            return text;
        }

        private void TryOpenConnection()
        {
            if (Connection.State.HasFlag(ConnectionState.Open)) return;

            Connection.Open();
            Log("Opened DbConnection");
        }

        private async Task TryOpenConnectionAsync()
        {
            if (Connection.State.HasFlag(ConnectionState.Open)) return;

            await Connection.OpenAsync();
            Log("Opened DbConnection Async");
        }

        private string GetUnprefixedParameterName(string parameterName)
        {
            return _database.Provider.Interpreter.GetUnprefixedParameterName(parameterName);
        }
    }
}
