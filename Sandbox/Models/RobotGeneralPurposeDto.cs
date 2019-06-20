using System;

namespace FluidDbClient.Sandbox.Models
{
    public class RobotGeneralPurposeDto
    {
        public int RobotId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public DateTime DateBuilt { get; set; }

        public DateTime? DateDestroyed { get; set; }

        public bool IsEvil { get; }

        public string ExtraPropertyOne { get; set; }

        public Guid ExtraPropertyTwo { get; set; }
    }
}
