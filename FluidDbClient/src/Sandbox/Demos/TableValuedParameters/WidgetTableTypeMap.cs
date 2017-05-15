using FluidDbClient.Sandbox.Models;
using FluidDbClient.Sql;

namespace FluidDbClient.Sandbox.Demos.TableValuedParameters
{
    public class WidgetTableTypeMap : TableTypeMap<Widget>
    {
        public WidgetTableTypeMap()
        {
            HasName("Widgets");

            Property(x => x.GlobalId)
                .IsInUniqueKey();
                
            Property(x => x.Name)
                .HasLength(100);

            Property(x => x.Description)
                .HasLength(500);
        }
    }
}
