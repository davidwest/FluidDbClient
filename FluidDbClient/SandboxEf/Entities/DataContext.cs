using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SandboxEf.Entities
{
    public class DataContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.AddAllEntityConfigurations();
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
