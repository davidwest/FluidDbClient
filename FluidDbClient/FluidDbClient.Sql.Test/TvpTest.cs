using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluidDbClient.Shell;
using FluidDbClient.Sql.Test.Entities;
using FluidDbClient.Sql.Test.TableTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluidDbClient.Sql.Test
{
    // TODO: break this up

    [TestClass]
    public class TvpTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            Initializer.Initialize();
        }

        [TestInitialize]
        public void Initialize()
        {
            Db.Execute("DELETE FROM Widget;");
        }

        [TestMethod]
        public void TableValueParameters_ShouldPersistCorrectly()
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
            
            CollectionAssert.AreEqual(expected, updated, new WidgetComparer());

            foreach (var w in updated)
            {
                Trace.WriteLine(w.ToDiagnosticString());
            }
        }

        private static void PersistNewWidgets(IEnumerable<Widget> widgets)
        {
            var data = widgets.ToStructuredData(new NewWidgetTableTypeMap());

            Db.Execute(InsertScript, new { data });
        }
        
        private static void PersistUpdatedWidgets(IEnumerable<Widget> widgets)
        {
            var data = widgets.ToStructuredData(new UpdatedWidgetTableTypeMap());

            Db.Execute(UpdateScript, new { data });
        }

        private static Widget[] GetSavedWidgets()
        {
            return Db.GetResultSet("SELECT * FROM Widget ORDER BY Id;").Map<Widget>().ToArray();
        }

        private static IEnumerable<Widget> GetSourceWidgets()
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
