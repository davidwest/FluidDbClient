using System.Data;
using FluidDbClient.Sql;
using SandboxEf.Entities;

namespace SandboxEf.TableTypes
{
    public class NewWidgetTableTypeMap : TableTypeMap<Widget>
    {
        public NewWidgetTableTypeMap()
        {
            HasName("NewWidget");

            Property(x => x.Id)
                .Ignore();

            Property(x => x.ExternalId)
                .HasBehavior(ColumnBehavior.UniqueKeyComponent);

            Property(x => x.IsArchived)
                .Ignore();

            Property(x => x.Name)
                .HasMaxLength(200);

            Property(x => x.Cost)
                .HasPrecision(18, 2);

            Property(x => x.ReleaseDate)
                .HasSqlType(SqlDbType.Date);
        }
    }
}
