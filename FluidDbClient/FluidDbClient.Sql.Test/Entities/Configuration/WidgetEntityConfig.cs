using System.Data.Entity.ModelConfiguration;

namespace FluidDbClient.Sql.Test.Entities
{
    public class WidgetEntityConfig : EntityTypeConfiguration<Widget>
    {
        public WidgetEntityConfig()
        {
            Property(x => x.ExternalId)
                .HasUniqueIndexAnnotation("UX_ExternalId");

            Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            Property(x => x.Cost)
                .HasPrecision(18, 2);

            Property(x => x.ReleaseDate)
                .HasColumnType("DATE");
            
            Property(x => x.SerialCode)
                .IsRequired()
                .HasMaxLength(null);
        }
    }
}
