using System.Collections.Generic;

namespace FluidDbClient.Sql.Test.Entities
{
    public class Component
    {
        public int Id { get; set; }
        
        public ComponentStyle Style { get; set; }

        public string Name { get; set; }

        public int CompositeId { get; set; }
        public virtual Composite Composite { get; set; }

        public Schedule MaintenanceSchedule { get; set; }

        public Schedule ReviewSchedule { get; set; }

        public virtual ICollection<Widget> Widgets { get; set; }
    }
}
