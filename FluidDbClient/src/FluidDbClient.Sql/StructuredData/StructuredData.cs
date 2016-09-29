
using System.Collections.Generic;
using Microsoft.SqlServer.Server;

namespace FluidDbClient.Sql
{
    public class StructuredData
    {
        public StructuredData(string tableTypeName, IReadOnlyCollection<SqlDataRecord> rows)
        {
            TableTypeName = tableTypeName;
            Rows = rows;
        }

        public string TableTypeName { get; }
        public IReadOnlyCollection<SqlDataRecord> Rows { get; }
    }
}
