
using System.Data;
using FluidDbClient.Sql;
using Microsoft.Data.SqlClient.Server;

namespace FluidDbClient.Sandbox
{
    public class IntegerIdsDataBuilder : StructuredDataBuilder
    {
        public IntegerIdsDataBuilder() : base("IntegerIds", new SqlMetaData("Id", SqlDbType.Int))
        { }
    }
}
