using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluidDbClient.Sql.Test.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluidDbClient.Sql.Test
{
    [TestClass]
    public class DataTableMappingTests
    {
        [TestMethod]
        public void EntitiesMapToDataTables()
        {
            var composites = GetSourceComposites();
            var components = composites.SelectMany(c => c.Components).ToArray();
            var widgets = components.SelectMany(c => c.Widgets).ToArray();

            var compositeDt = 
                composites
                    .Select(c => new
                    {
                        c.Id,
                        c.Name
                    })
                    .ToDataTable(nameof(Composite));

            var componentDt = 
                components
                    .Select(c => new
                    {
                        c.Id,
                        c.CompositeId,
                        c.Style,
                        c.Name,
                        MaintenanceSchedule_Frequency = c.MaintenanceSchedule.Frequency,
                        MaintenanceSchedule_StartDate = c.MaintenanceSchedule.StartDate,
                        ReviewSchedule_Frequency = c.ReviewSchedule.Frequency,
                        ReviewSchedule_StartDate = c.ReviewSchedule.StartDate
                    })
                    .ToDataTable(nameof(Component));

            var widgetDt = widgets.ToDataTable();
            
            Trace.WriteLine(compositeDt.ToDiagnosticString());
            Trace.WriteLine(componentDt.ToDiagnosticString());
            Trace.WriteLine(widgetDt.ToDiagnosticString());
        }

        private static Composite[] GetSourceComposites()
        {
            var widgets = new[]
            {
                new Widget
                {
                    ExternalId = Guid.NewGuid(),
                    Environment = WidgetEnvironment.Household,
                    Name = "Slicer",
                    Cost = (decimal)213.67,
                    CreatedTimestamp = DateTimeOffset.UtcNow,
                    ReleaseDate = DateTime.UtcNow.AddDays(5).Date,
                    Weight = 11.42,
                    Rating = 3.768f,
                    SerialCode = new byte[]{11, 101, 239, 12, 61}
                },
                new Widget
                {
                    ExternalId = Guid.NewGuid(),
                    Environment = WidgetEnvironment.Industrial,
                    Name = "Dicer",
                    Cost = (decimal)55.80,
                    CreatedTimestamp = DateTimeOffset.UtcNow,
                    ReleaseDate = DateTime.UtcNow.AddDays(5).Date,
                    Weight = 5.554,
                    SerialCode = new byte[]{1, 2, 33, 53, 249}
                },
                new Widget
                {
                    ExternalId = Guid.NewGuid(),
                    Environment = WidgetEnvironment.Industrial,
                    Name = "Masher",
                    Cost = (decimal)121.63,
                    CreatedTimestamp = DateTimeOffset.UtcNow,
                    ReleaseDate = DateTime.UtcNow.AddDays(5).Date,
                    Weight = 17.9,
                    Rating = 2.931f,
                    SerialCode = new byte[]{171, 3, 3, 206, 79, 64, 5}
                }
            };

            var composites = new[]
            {
                new Composite
                {
                    Name = "Composite A",
                    Components = new List<Component>
                    {
                        new Component
                        {
                            Name = "Component A-1",
                            Style = ComponentStyle.Nice,
                            MaintenanceSchedule = new Schedule
                            {
                                Frequency = ScheduleFrequency.Monthly,
                                StartDate = DateTime.UtcNow.AddDays(3)
                            },
                            ReviewSchedule = new Schedule
                            {
                                Frequency = ScheduleFrequency.Weekly,
                                StartDate = DateTime.UtcNow.AddDays(4)
                            },
                            Widgets = new List<Widget>{widgets[0], widgets[1]}
                        },
                        new Component
                        {
                            Name = "Component A-2",
                            Style = ComponentStyle.Easy,
                            MaintenanceSchedule = new Schedule
                            {
                                Frequency = ScheduleFrequency.Daily,
                                StartDate = DateTime.UtcNow.AddDays(17)
                            },
                            ReviewSchedule = new Schedule
                            {
                                Frequency = ScheduleFrequency.Yearly,
                                StartDate = DateTime.UtcNow.AddDays(1)
                            },
                            Widgets = new List<Widget>{widgets[0], widgets[2]}
                        }
                    }
                },
                new Composite
                {
                    Name = "Composite B",
                    Components = new List<Component>
                    {
                        new Component
                        {
                            Name = "Component B-1",
                            Style = ComponentStyle.Cool,
                            MaintenanceSchedule = new Schedule
                            {
                                Frequency = ScheduleFrequency.Daily,
                                StartDate = DateTime.UtcNow.AddDays(10)
                            },
                            ReviewSchedule = new Schedule
                            {
                                Frequency = ScheduleFrequency.Daily,
                                StartDate = DateTime.UtcNow.AddDays(30)
                            },
                            Widgets = new List<Widget>{widgets[1], widgets[2]}
                        },
                        new Component
                        {
                            Name = "Component B-2",
                            Style = ComponentStyle.Swell,
                            MaintenanceSchedule = new Schedule
                            {
                                Frequency = ScheduleFrequency.Monthly,
                                StartDate = DateTime.UtcNow.AddDays(7)
                            },
                            ReviewSchedule = new Schedule
                            {
                                Frequency = ScheduleFrequency.Weekly,
                                StartDate = DateTime.UtcNow.AddDays(20)
                            },
                            Widgets = new List<Widget>{widgets[0]}
                        }
                    }
                }
            };

            return composites;
        }
    }
}
