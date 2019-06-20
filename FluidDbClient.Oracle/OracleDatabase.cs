using System;

namespace FluidDbClient.Oracle
{
    public class OracleDatabase : Database
    {
        public OracleDatabase(string name, string connectionString, Action<string> log = null) 
            : base(name, connectionString, new OracleDbProvider(connectionString), log)
        { }
    }
}
