using System.Diagnostics;
using FluidDbClient.Sandbox.Models;
using FluidDbClient.Sql;

namespace FluidDbClient.Sandbox
{
    public static class Initializer
    {
        public static void Initialize()
        {
            const string connString = @"Server = (LocalDb)\MSSQLLocalDB; Initial Catalog = Acme; Trusted_Connection = true;";
            
            DbRegistry.Initialize(new AcmeDb(connString, msg => Debug.WriteLine(msg)));
            
            TableTypeRegistry.Register(new RobotsTableTypeMap(), 
                                       new WidgetsTableTypeMap());
        }
    }

    public class WidgetsTableTypeMap : TableTypeMap<Widget>
    {
        public WidgetsTableTypeMap()
        {
            HasName("Widgets");

            Property(x => x.GlobalId).IsInUniqueKey();
            Property(x => x.Name).HasLength(100);
            Property(x => x.Description).HasLength(500);
        }
    }
    
    public class RobotsTableTypeMap : TableTypeMap<RobotGeneralPurposeDto>
    {
        public RobotsTableTypeMap()
        {
            HasName("Robots");
            
            Property(x => x.Name).HasLength(100);
            Property(x => x.Description).HasLength(500);
            Property(x => x.ExtraPropertyOne).Ignore();
            Property(x => x.ExtraPropertyTwo).Ignore();
        }
    }
}
