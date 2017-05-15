using System;

namespace FluidDbClient.Sql
{
    public class SqlDatabase : Database
    {
        public SqlDatabase(string name, string connectionString, Action<string> log = null) 
            : base(name, connectionString, new SqlDbProvider(), log)
        { }
    }
}
