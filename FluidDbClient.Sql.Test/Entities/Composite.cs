using System.Collections.Generic;

namespace FluidDbClient.Sql.Test.Entities
{
    public class Composite
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Component> Components { get; set; }
    }
}
