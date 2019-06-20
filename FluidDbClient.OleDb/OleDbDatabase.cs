using System;

namespace FluidDbClient.OleDb
{
    public class OleDbDatabase : Database
    {
        public OleDbDatabase(string name, string connectionString, Action<string> log = null) 
            : base(name, connectionString, new OleDbProvider(connectionString), log)
        { }
    }
}
