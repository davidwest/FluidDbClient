using System;
using FluidDbClient.Sql;

namespace FluidDbClient.Sandbox
{
    public class AcmeDb : Database
    {
        public AcmeDb(string connectionString, Action<string> log = null) 
            : base("AcmeDb", connectionString, new SqlDbProvider(), log)
        { }
    }
}
