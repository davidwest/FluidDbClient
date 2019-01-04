using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using FluidDbClient.Shell;
using FluidDbClient.Sql.Test.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluidDbClient.Sql.Test
{
    [TestClass]
    public class DataSetTests
    {
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            Initializer.Initialize();

            var composites = GetSourceComposites();

            SaveComposites(composites);
        }

        [TestMethod]
        public void RetrievedDataTable_IncludesExpectedRows()
        {
            var dataTable = Db.GetDataTable("SELECT * FROM Component", nameof(Component));

            RetrievedDataTable_IncludesExpectedRows(dataTable);
        }

        [TestMethod]
        public void RetrievedDataTable_IncludesExpectedRows_Async()
        {
            var dataTable = Db.GetDataTableAsync("SELECT * FROM Component", nameof(Component));
            
            RetrievedDataTable_IncludesExpectedRows(dataTable.Result);
        }

        [TestMethod]
        public void RetrievedDataSet_IncludesExpectedTablesAndRows()
        {
            var dataSet = Db.GetDataSet(QueryScript, new[] { nameof(Composite), nameof(Component), nameof(Widget), "ComponentWidget" });

            RetrievedDataSet_IncludesExpectedTablesAndRows(dataSet);
        }
        
        [TestMethod]
        public void RetrievedDataSet_IncludesExpectedTablesAndRows_Async()
        {
            var dataSet = Db.GetDataSetAsync(QueryScript, new[] { nameof(Composite), nameof(Component), nameof(Widget), "ComponentWidget" });

            RetrievedDataSet_IncludesExpectedTablesAndRows(dataSet.Result);
        }

        private static void RetrievedDataTable_IncludesExpectedRows(DataTable dataTable)
        {
            Trace.WriteLine(dataTable.ToXml());

            Assert.AreEqual(4, dataTable.Rows.Count);
        }

        private static void RetrievedDataSet_IncludesExpectedTablesAndRows(DataSet dataSet)
        {
            Trace.WriteLine(dataSet.ToXml());

            Assert.AreEqual(2, dataSet.Tables[nameof(Composite)].Rows.Count);
            Assert.AreEqual(4, dataSet.Tables[nameof(Component)].Rows.Count);
            Assert.AreEqual(3, dataSet.Tables[nameof(Widget)].Rows.Count);
            Assert.AreEqual(7, dataSet.Tables["ComponentWidget"].Rows.Count);
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
SELECT * FROM Composite;
SELECT * FROM Component;
SELECT * FROM Widget;
SELECT * FROM ComponentWidget;
";
    }
}
