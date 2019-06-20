using System.Data.Entity.ModelConfiguration;

namespace FluidDbClient.Sql.Test.Entities
{
    public class ScheduleConfig : ComplexTypeConfiguration<Schedule>
    {
        public ScheduleConfig()
        {
            Property(x => x.StartDate)
                .HasColumnType("DATE");
        }
    }
}
