
using System;
using System.Data;

namespace FluidDbClient
{
    public class DbSession : DbSessionBase
    {
        public DbSession(IsolationLevel isolationLevel, Action<string> log = null) 
            : base(DbRegistry.GetDatabase(), isolationLevel, log)
        { }

        public DbSession(Action<string> log = null) 
            : this(IsolationLevel.ReadCommitted, log)
        { }
    }


    public class DbSession<TDatabase> : DbSessionBase where TDatabase : Database
    {
        public DbSession(IsolationLevel isolationLevel, Action<string> log = null) 
            : base(DbRegistry.GetDatabase<TDatabase>(), isolationLevel, log)
        { }

        public DbSession(Action<string> log = null) 
            : this(IsolationLevel.ReadCommitted, log)
        { }
    }
}
