using System;

namespace FluidDbClient.Sandbox.Models
{
    public class Widget
    {
        public Widget(int widgetId, Guid globalId, string name, string description)
        {
            WidgetId = widgetId;
            GlobalId = globalId;
            Name = name;
            Description = description;
        }

        public int WidgetId { get; }
        public Guid GlobalId { get; }
        public string Name { get; }
        public string Description { get; }
    }
}
