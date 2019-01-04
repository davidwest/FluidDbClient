using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FluidDbClient.Sql.Test
{
    public class DataContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.AddAllTypeConfigurations();

            base.OnModelCreating(modelBuilder);
        }
    }
}
