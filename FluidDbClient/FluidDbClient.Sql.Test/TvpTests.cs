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
        public void EmptyTableValueParameter_PersistsNothing()
        {
            var sourceWidgets = new Widget[0];

            var data = sourceWidgets.ToStructuredData(new NewWidgetTableTypeMap());

            Db.Execute(InsertScript, new { data });

            var savedWidgets = GetSavedWidgets();

            CollectionAssert.AreEqual(sourceWidgets, savedWidgets, new WidgetValueComparer());
        }
        
        [TestMethod]
        public void TableValueParameter_PersistsCorrectly()
        {
            var opResult = DoAddAndUpdateOperations();

            var expected = opResult.Item1;
            var updated = opResult.Item2;

            foreach (var w in updated)
            {
                Trace.WriteLine(w.ToDiagnosticString());
            }

            CollectionAssert.AreEqual(expected, updated, new WidgetIdentityAndValueComparer());
        }
        
        [TestMethod]
        public void TableValueParameter_PersistsCorrectly_Async()
        {
            var opResult = DoAddAndUpdateOperationsAsync().Result;

            var expected = opResult.Item1;
            var updated = opResult.Item2;

            foreach (var w in updated)
            {
                Trace.WriteLine(w.ToDiagnosticString());
            }

            CollectionAssert.AreEqual(expected, updated, new WidgetIdentityAndValueComparer());
        }
        
        [TestMethod]
        public void TableValueParameter_SourcedFromCompatibleObjects_WithStrictTypeMapping_PersistsCorrectly()
        {
            var sourceWidgets = GetSourceWidgets();

            var data =
                sourceWidgets
                    .Select(w => new
                    {
                        // NOTE:
                        // * properties are out of order relative to Widget
                        // * extra properties have been added

                        ExtraProperty1 = "Hello World!",
                        ExtraProperty2 = 100,
                        w.Rating,
                        w.Weight,
                        w.SerialCode,
                        w.ReleaseDate,
                        w.CreatedTimestamp,
                        w.Cost,
                        w.Name,
                        w.Environment,
                        w.IsArchived,
                        w.ExternalId
                    })
                    .ToStructuredData(new NewWidgetTableTypeMap());

            Db.Execute(InsertScript, new { data });

            var savedWidgets = GetSavedWidgets();
            
            CollectionAssert.AreEqual(sourceWidgets, savedWidgets, new WidgetValueComparer());
        }

        [TestMethod]
        public void TableValueParameter_SourcedFromNearlyCompatibleObjects_WithCoercedTypeMapping_PersistsCorrectly()
        {
            var sourceWidgets = GetSourceWidgets();

            var data = 
                sourceWidgets
                    .Select(w => new 
                    {
                        // NOTE:
                        // * properties are out of order relative to Widget
                        // * property types differ
                        // * extra properties have been added

                        ExtraProperty1 = "Hello World!",
                        ExtraProperty2 = 100,
                        Rating = (decimal?)w.Rating,
                        Weight = (float?)w.Weight,
                        w.SerialCode,
                        w.ReleaseDate,
                        w.CreatedTimestamp,
                        Cost = (float)w.Cost,
                        w.Name,
                        Environment = (long)w.Environment,
                        w.IsArchived,
                        w.ExternalId
                    })
                    .ToStructuredData(new NewWidgetTableTypeMap(), TypeMapOption.Coerce);
            
            Db.Execute(InsertScript, new {data});

            var savedWidgets = GetSavedWidgets();

            foreach (var w in savedWidgets)
            {
                Trace.WriteLine(w.ToDiagnosticString());
            }

            // NOTE: we cannot assert equal property values due to casting in intermediate objects

            Assert.AreEqual(sourceWidgets.Length, savedWidgets.Length);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void TableValueParameter_SourcedFromNearlyCompatibleObjects_WithStrictTypeMapping_ThrowsException()
        {
            var sourceWidgets = GetSourceWidgets();

            var data =
                sourceWidgets
                    .Select(w => new
                    {
                        // NOTE:
                        // * properties are out of order relative to Widget
                        // * property types differ
                        // * extra properties have been added

                        ExtraProperty1 = "Hello World!",
                        ExtraProperty2 = 100,
                        Rating = (decimal?)w.Rating,
                        Weight = (float?)w.Weight,
                        w.SerialCode,
                        w.ReleaseDate,
                        w.CreatedTimestamp,
                        Cost = (float)w.Cost,
                        w.Name,
                        Environment = (long)w.Environment,
                        w.IsArchived,
                        w.ExternalId
                    })
                    .ToStructuredData(new NewWidgetTableTypeMap());

            Db.Execute(InsertScript, new { data });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "\"ExternalId\" is required")]
        public void TableValueParameter_SourcedFromIncompatibleObjects_ShouldThrowException()
        {
            var sourceWidgets = GetSourceWidgets();

            var data =
                sourceWidgets
                    .Select(w => new
                    {                        
                        ExtraProperty1 = "Hello World!",
                        ExtraProperty2 = 100,
                        w.Rating,
                        w.Weight,
                        w.SerialCode,
                        w.ReleaseDate,
                        w.CreatedTimestamp,
                        w.Cost,
                        w.Name,
                        w.Environment,
                        w.IsArchived

                        // uh oh: no ExternalId

                    })
                    .ToStructuredData(new NewWidgetTableTypeMap());

            Db.Execute(InsertScript, new { data });
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
            return Db.GetResultSet("SELECT * FROM Widget ORDER BY Name;").Map<Widget>().ToArray();
        }

        private static async Task<Widget[]> GetSavedWidgetsAsync()
        {
            return (await Db.CollectResultSetAsync("SELECT * FROM Widget ORDER BY Name;")).Map<Widget>().ToArray();
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
            }.OrderBy(w => w.Name).ToArray();
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
