using System;

namespace FluidDbClient.Sandbox.Models
{
    public class Robot
    {
        public Robot(int robotId, string name, string description, DateTime dateBuilt, DateTime? dateDestroyed, bool isEvil)
        {
            RobotId = robotId;
            Name = name;
            Description = description;
            DateBuilt = dateBuilt;
            DateDestroyed = dateDestroyed;
            IsEvil = isEvil;
        }

        public int RobotId { get; }
        public string Name { get; }
        public string Description { get; }
        public DateTime DateBuilt { get; }
        public DateTime? DateDestroyed { get; }
        public bool IsEvil { get; }
    }    
}
