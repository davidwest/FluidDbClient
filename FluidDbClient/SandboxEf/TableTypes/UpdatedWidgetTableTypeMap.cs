using System.Data;
using FluidDbClient.Sql;
using SandboxEf.Entities;

namespace SandboxEf.TableTypes
{
    public class UpdatedWidgetTableTypeMap : TableTypeMap<Widget>
    {
        public UpdatedWidgetTableTypeMap()
        {
            HasName("UpdatedWidget");

            Property(x => x.Id)
                .IsInUniqueKey();

            Property(x => x.ExternalId)
                .HasBehavior(ColumnBehavior.PrimaryKeyComponent);

            Property(x => x.Name)
                .HasMaxLength(200);

            Property(x => x.Cost)
                .HasPrecision(18, 2);

            Property(x => x.CreatedTimestamp)
                .Ignore();

            Property(x => x.ReleaseDate)
                .HasSqlType(SqlDbType.Date);
        }
    }
}
