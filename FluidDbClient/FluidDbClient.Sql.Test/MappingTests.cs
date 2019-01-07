using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluidDbClient.Shell;
using FluidDbClient.Sql.Test.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluidDbClient.Sql.Test
{
    // TODO: add more granular tests

    [TestClass]
    public class MappingTests
    {
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            Initializer.Initialize();

            var composites = GetSourceComposites();

            SaveComposites(composites);
        }
        
        [TestMethod]
        public void MappedObjects_HaveIdenticalData_WithThoseRetrievedByDbContext()
        {
            var expected = GetSavedCompositesEf();

            var actual = GetSavedComposites();

            foreach (var c in actual)
            {
                Trace.WriteLine(c.ToDiagnosticString());
            }

            CollectionAssert.AreEqual(expected, actual, new CompositeIdentityAndValueComparer());
        }
        
        [TestMethod]
        public void MappedObjects_HaveIdenticalData_WithThoseRetrievedByDbContext_Async()
        {
            var expected = GetSavedCompositesEf();

            var actual = GetSavedCompositesAsync().Result;

            foreach (var c in actual)
            {
                Trace.WriteLine(c.ToDiagnosticString());
            }

            CollectionAssert.AreEqual(expected, actual, new CompositeIdentityAndValueComparer());
        }

        private static Composite[] GetSavedComposites()
        {
            return MapToComposites(Db.GetResultSet(QueryScript).Buffer());
        }
        
        private static async Task<Composite[]> GetSavedCompositesAsync()
        {
            return MapToComposites(await Db.CollectResultSetAsync(QueryScript));
        }

        private static Composite[] MapToComposites(IEnumerable<IDataRecord> records)
        {
            var result =
                records
                .MapNested(
                    r => r.MapProperty<Composite, int>(c => c.Id),
                    r => r.MapProperty<Component, int>(c => c.Id),
                    r => r.MapProperty<Widget, int>(w => w.Id),
                    (r, components) =>
                    {
                        var composite = r.Map<Composite>();
                        composite.Components = components.ToList();
                        return composite;
                    },
                    (r, widgets) =>
                    {
                        var component =
                            r.Map(
                                rec => rec.Map<Schedule>(nameof(Component.MaintenanceSchedule)),
                                rec => rec.Map<Schedule>(nameof(Component.ReviewSchedule)),
                                (rec, ms, rs) =>
                                {
                                    var cmp = rec.Map<Component>();
                                    cmp.MaintenanceSchedule = ms;
                                    cmp.ReviewSchedule = rs;
                                    return cmp;
                                });

                        component.Widgets = widgets.ToList();
                        return component;
                    },
                    r => r.Map<Widget>());

            return result.OrderBy(c => c.Id).ToArray();
        }

        private static Composite[] GetSavedCompositesEf()
        {
            using (var dbContext = new DataContext())
            {
                return 
                    dbContext.Set<Composite>()
                        .Include(cs => cs.Components.Select(ct => ct.Widgets))
                        .OrderBy(c => c.Id)
                        .AsNoTracking()
                        .ToArray();
            }
        }

        private static void SaveComposites(IEnumerable<Composite> composites)
        {
            using (var dbContext = new DataContext())
            {
                foreach (var composite in composites)
                {
                    dbContext.Set<Composite>().Add(composite);

                    dbContext.SaveChanges();
                }
            }
        }

        private static IEnumerable<Composite> GetSourceComposites()
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

        private const string QueryScript =
@"
SELECT cs.Id AS Composite_Id, cs.[Name] AS Composite_Name,
        cp.Id AS Component_Id, cp.[Name] AS Component_Name, cp.Style,
        cp.MaintenanceSchedule_Frequency, cp.MaintenanceSchedule_StartDate,
        cp.ReviewSchedule_Frequency, cp.ReviewSchedule_StartDate,
        w.Id AS Widget_Id, w.ExternalId, w.[Environment], w.IsArchived, w.[Name] AS Widget_Name, 
        w.Cost, w.CreatedTimestamp, w.ReleaseDate, w.Rating, w.Weight, w.SerialCode
FROM Composite AS cs
LEFT JOIN Component AS cp ON cp.CompositeId = cs.Id
LEFT JOIN ComponentWidget AS cw ON cp.Id = cw.Component_Id
LEFT JOIN Widget AS w ON cw.Widget_Id = w.Id;
";
    }
}
