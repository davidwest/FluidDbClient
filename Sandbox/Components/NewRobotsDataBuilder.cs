using System.Data;
using FluidDbClient.Sql;
using Microsoft.Data.SqlClient.Server;

namespace FluidDbClient.Sandbox
{
    public class NewRobotsDataBuilder : StructuredDataBuilder
    {
        public NewRobotsDataBuilder() :
            base("NewRobots", 
                 new SqlMetaData("Name", SqlDbType.NVarChar, 50),
                 new SqlMetaData("Description", SqlDbType.NVarChar, 1000),
                 new SqlMetaData("DateBuilt", SqlDbType.Date),
                 new SqlMetaData("IsEvil", SqlDbType.Bit))
        { }
    }
}
