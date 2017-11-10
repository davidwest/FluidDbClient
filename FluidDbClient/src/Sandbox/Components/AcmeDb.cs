using System;
using FluidDbClient.Sql;

namespace FluidDbClient.Sandbox
{
    public class AcmeDb : SqlDatabase
    {
        public AcmeDb(string connectionString, Action<string> log = null) 
            : base("AcmeDb", connectionString, log)
        { }
    }

    public class BubbleWrapDb : SqlDatabase
    {
        public BubbleWrapDb(string connectionString, Action<string> log = null) 
            : base("BubbleWrapDb", connectionString, log)
        { }
    }
}
