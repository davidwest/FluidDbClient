using System;
using FluidDbClient.Sql;

namespace FluidDbClient.Sandbox
{
    public class AcmeDb : Database
    {
        public AcmeDb(string connectionString, Action<string> log = null) 
            : base("Acme Database", connectionString, new SqlDbProvider(), log)
        { }
    }
}
