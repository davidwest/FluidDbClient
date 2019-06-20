using System;

namespace FluidDbClient.Sql.Test.Entities
{
    public class Widget
    {
        public int Id { get; set; }

        public Guid ExternalId { get; set; }

        public bool IsArchived { get; set; }

        public WidgetEnvironment Environment { get; set; } = WidgetEnvironment.Household;

        public string Name { get; set; }
        
        public decimal Cost { get; set; }

        public DateTimeOffset CreatedTimestamp { get; set; }

        public DateTime ReleaseDate { get; set; }
        
        public byte[] SerialCode { get; set; } = new byte[0];

        public double Weight { get; set; }

        public float? Rating { get; set; }
    }
}
