﻿using System.Data.Common;
using System.Data.SqlClient;

namespace FluidDbClient.Sql
{
    public class SqlDbProvider : DbProvider
    {
        public SqlDbProvider() : base("System.Data.SqlClient")
        {
            Interpreter = new SqlValueInterpreter();
        }

        public override DbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
        
        public override DbParameter CreateParameter(string name, object value)
        {
            return SqlParameterFactory.CreateParameter(name, value);
        }

        public override IDbProviderValueInterpreter Interpreter { get; }

        protected override DbConnection GetConnectionUsing(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
