using System.Collections.Generic;
using Microsoft.SqlServer.Server;

namespace FluidDbClient.Sql
{
    public class StructuredData
    {
        public StructuredData(string tableTypeName, IEnumerable<SqlDataRecord> records)
        {
            TableTypeName = tableTypeName;
            Records = records;
        }
        
        public string TableTypeName { get; }
        public IEnumerable<SqlDataRecord> Records { get; }
    }
}
