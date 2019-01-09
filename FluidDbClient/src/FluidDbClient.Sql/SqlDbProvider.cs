using System.Data.Common;
using System.Data.SqlClient;

namespace FluidDbClient.Sql
{
    internal class SqlDbProvider : IDbProvider
    {
        private readonly string _connectionString;

        public SqlDbProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string ProviderId => "System.Data.SqlClient";

        public DbConnection CreateConnection() => new SqlConnection(_connectionString);

        public DbParameter CreateParameter(string name, object value) => SqlParameterFactory.CreateParameter(name, value);

        public IDbProviderValueInterpreter Interpreter { get; } = new SqlValueInterpreter();
    }
}
