
using System;
using System.Data;
using FluidDbClient.Sandbox.Models;

namespace FluidDbClient.Sandbox
{
    public static class ModelMappingExtensions
    {
        public static Robot MapToRobot(this IDataRecord rec)
        {
            return new Robot(rec.Get<int>("RobotId"), 
                             rec.Get<string>("Name"), 
                             rec.Get<string>("Description"), 
                             rec.Get<DateTime>("DateBuilt"), 
                             rec.Get<DateTime?>("DateDestroyed"), 
                             rec.Get<bool>("IsEvil"));
        }

        public static Widget MapToWidget(this IDataRecord rec)
        {
            return new Widget(rec.Get<int>("WidgetId"),
                              rec.Get<Guid>("GlobalId"),
                              rec.Get<string>("Name"),
                              rec.Get<string>("Description"));
        }
    }
}
