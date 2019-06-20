using System.Data;
using FluidDbClient.Sql.Test.Entities;

namespace FluidDbClient.Sql.Test.TableTypes
{
    public class UpdatedWidgetTableTypeMap : TableTypeMap<Widget>
    {
        public UpdatedWidgetTableTypeMap()
        {
            HasName("UpdatedWidget");

            Property(x => x.Id)
                .HasBehavior(ColumnBehavior.PrimaryKeyComponent);

            Property(x => x.ExternalId)
                .HasBehavior(ColumnBehavior.UniqueKeyComponent);

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
