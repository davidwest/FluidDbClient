
using System.Data;
using FluidDbClient.Sql;
using Microsoft.Data.SqlClient.Server;

namespace FluidDbClient.Sandbox
{
    public class NewWidgetsDataBuilder : StructuredDataBuilder
    {
        public NewWidgetsDataBuilder() :
            base("NewWidgets",
                 new SqlMetaData("GlobalId", SqlDbType.UniqueIdentifier), 
                 new SqlMetaData("Name", SqlDbType.NVarChar, 50),
                 new SqlMetaData("Description", SqlDbType.NVarChar, -1))
        { }
    }
}
