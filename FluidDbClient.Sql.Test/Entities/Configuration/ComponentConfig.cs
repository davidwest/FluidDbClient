using System.Data.Entity.ModelConfiguration;

namespace FluidDbClient.Sql.Test.Entities
{
    public class ComponentConfig : EntityTypeConfiguration<Component>
    {
        public ComponentConfig()
        {
            Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(80);

            HasRequired(x => x.Composite)
                .WithMany(x => x.Components)
                .HasForeignKey(x => x.CompositeId);

            HasMany(x => x.Widgets)
                .WithMany();
        }
    }
}
