
using System.Data;
using Microsoft.SqlServer.Server;
using FluidDbClient.Sql;

namespace FluidDbClient.Sandbox
{
    public class IntegerIdsDataBuilder : StructuredDataBuilder
    {
        public IntegerIdsDataBuilder() : base("IntegerIds", new SqlMetaData("Id", SqlDbType.Int))
        { }
    }
}
