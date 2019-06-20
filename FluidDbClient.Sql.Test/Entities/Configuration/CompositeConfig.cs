using System.Data.Entity.ModelConfiguration;

namespace FluidDbClient.Sql.Test.Entities
{
    public class CompositeConfig : EntityTypeConfiguration<Composite>
    {
        public CompositeConfig()
        {
            Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(80);

            HasMany(x => x.Components)
                .WithRequired(x => x.Composite)
                .HasForeignKey(x => x.CompositeId);
        }
    }
}
