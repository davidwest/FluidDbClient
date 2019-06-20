using System;

namespace FluidDbClient
{
    public class DefaultDatabase : Database
    {
        public DefaultDatabase(
            string name, 
            string connectionString, 
            IDbProvider provider,
            Action<string> log = null) 
            : base(name, connectionString, provider, log)
        { }
    }
}
