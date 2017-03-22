using System.Collections.Generic;
using System.Data.Common;

namespace FluidDbClient
{
    public class DbScriptDocument
    {
        public DbScriptDocument(string text, IReadOnlyList<DbParameter> parameters)
        {
            Text = text;
            Parameters = parameters;
        }

        public string Text { get; }
        public IReadOnlyList<DbParameter> Parameters { get; }
    }
}
