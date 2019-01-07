using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FluidDbClient.Shell;
using FluidDbClient.Sql.Test.Entities;
using FluidDbClient.Sql.Test.TableTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluidDbClient.Sql.Test
{
    // TODO: add more granular tests

    [TestClass]
    public class TvpTests
    {
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            Initializer.Initialize();
        }

        [TestInitialize]
        public void Initialize()
        {
            DeleteAllWidgets();
        }

        [TestMethod]
        public void TableValueParameters_ShouldPersistCorrectly()
        {
            var opResult = DoAddAndUpdateOperations();

            var expected = opResult.Item1;
            var updated = opResult.Item2;

            foreach (var w in updated)
            {
                Trace.WriteLine(w.ToDiagnosticString());
            }

            CollectionAssert.AreEqual(expected, updated, new WidgetComparer());
        }
        
        [TestMethod]
        public void TableValueParameters_ShouldPersistCorrectly_Async()
        {
            var opResult = DoAddAndUpdateOperationsAsync().Result;

            var expected = opResult.Item1;
            var updated = opResult.Item2;

            foreach (var w in updated)
            {
                Trace.WriteLine(w.ToDiagnosticString());
            }

            CollectionAssert.AreEqual(expected, updated, new WidgetComparer());
        }

        private static Tuple<Widget[], Widget[]> DoAddAndUpdateOperations()
        {
            var sourceWidgets = GetSourceWidgets();

            PersistNewWidgets(sourceWidgets);

            var updated = GetSavedWidgets();

            foreach (var w in updated)
            {
                w.Name = w.Name + " (edited)";
                w.Cost += 5;
            }

            PersistUpdatedWidgets(updated);

            var expected = GetSavedWidgets();

            return new Tuple<Widget[], Widget[]>(expected, updated);
        }

        private static async Task<Tuple<Widget[],Widget[]>> DoAddAndUpdateOperationsAsync()
        {
            var sourceWidgets = GetSourceWidgets();

            await PersistNewWidgetsAsync(sourceWidgets);

            var updated = await GetSavedWidgetsAsync();

            foreach (var w in updated)
            {
                w.Name = w.Name + " (edited)";
                w.Cost += 5;
            }

            await PersistUpdatedWidgetsAsync(updated);

            var expected = await GetSavedWidgetsAsync();

            return new Tuple<Widget[], Widget[]>(expected,updated);
        }

        private static void PersistNewWidgets(IEnumerable<Widget> widgets)
        {
            var data = widgets.ToStructuredData(new NewWidgetTableTypeMap());

            Db.Execute(InsertScript, new { data });
        }

        private static async Task PersistNewWidgetsAsync(IEnumerable<Widget> widgets)
        {
            var data = widgets.ToStructuredData(new NewWidgetTableTypeMap());
            
            await Db.ExecuteAsync(InsertScript, new { data });
        }

        private static void PersistUpdatedWidgets(IEnumerable<Widget> widgets)
        {
            var data = widgets.ToStructuredData(new UpdatedWidgetTableTypeMap());

            Db.Execute(UpdateScript, new { data });
        }
        
        private static async Task PersistUpdatedWidgetsAsync(IEnumerable<Widget> widgets)
        {
            var data = widgets.ToStructuredData(new UpdatedWidgetTableTypeMap());

            await Db.ExecuteAsync(UpdateScript, new { data });
        }

        private static Widget[] GetSavedWidgets()
        {
            return Db.GetResultSet("SELECT * FROM Widget ORDER BY Id;").Map<Widget>().ToArray();
        }

        private static async Task<Widget[]> GetSavedWidgetsAsync()
        {
            return (await Db.CollectResultSetAsync("SELECT * FROM Widget ORDER BY Id;")).Map<Widget>().ToArray();
        }

        private static void DeleteAllWidgets()
        {
            Db.Execute("DELETE FROM Widget;");
        }

        private static Widget[] GetSourceWidgets()
        {
            return new[]
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
                }
            };
        }

        private const string InsertScript =
            @"
INSERT INTO Widget (ExternalId,[Environment],IsArchived,[Name],Cost,CreatedTimestamp,ReleaseDate,Weight,Rating,Serialcode) 
SELECT ExternalId,[Environment],0,[Name],Cost,CreatedTimestamp,ReleaseDate,Weight,Rating,SerialCode 
FROM @data;
";

        private const string UpdateScript =
            @"
UPDATE target
    SET target.[Name] = source.[Name], target.Cost = source.Cost
FROM Widget AS target
INNER JOIN @data AS source ON target.Id = source.Id;
";
    }
}
