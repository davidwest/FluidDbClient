using System;

namespace SandboxEf.Entities
{
    public class Widget
    {
        public int Id { get; set; }

        public Guid ExternalId { get; set; }

        public bool IsArchived { get; set; }

        public string Name { get; set; }
        
        public decimal Cost { get; set; }

        public DateTimeOffset CreatedTimestamp { get; set; }

        public DateTime ReleaseDate { get; set; }
        
        public byte[] SerialCode { get; set; }

        public double Weight { get; set; }

        public float? Rating { get; set; }
    }
}
