using System;

namespace FluidDbClient
{
    public abstract class Database
    {
        private readonly Action<string> _log;

        protected Database(string name, string connectionString, IDbProvider provider, Action<string> log = null)
        {
            Name = name;
            ConnectionString = connectionString;
            Provider = provider;

            _log = log ?? (msg => { });
        }

        public string Name { get; }

        public string ConnectionString { get; }

        public IDbProvider Provider { get; }

        internal void Log(string msg)
        {
            _log($"{Name} :: {msg}");
        }
    }
}
