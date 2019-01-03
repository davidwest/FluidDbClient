using System;

namespace FluidDbClient.Sql.Test.Entities
{
    public class Schedule
    {
        public ScheduleFrequency Frequency { get; set; }

        public DateTime StartDate { get; set; }
    }
}
