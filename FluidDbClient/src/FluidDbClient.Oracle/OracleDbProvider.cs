using System.Data.Common;
using Oracle.ManagedDataAccess.Client;

namespace FluidDbClient.Oracle
{
    internal class OracleDbProvider : IDbProvider
    {
        private readonly string _connectionString;

        public OracleDbProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbConnection CreateConnection() => new OracleConnection(_connectionString);

        public DbParameter CreateParameter(string name, object value) => OracleParameterFactory.CreateParameter(name, value);

        public DbCommand CreateCommand(DbConnection connection)
        {
            var cmd = connection.CreateCommand();

            ((OracleCommand) cmd).BindByName = true;

            return cmd;
        }

        public string ProviderId => "Oracle.ManagedDataAccess.Client";

        public IDbProviderValueInterpreter Interpreter => new OracleValueInterpreter();
    }
}
