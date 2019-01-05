using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluidDbClient.Shell;
using FluidDbClient.Sql.Test.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluidDbClient.Sql.Test
{
    [TestClass]
    public class BulkInsertTests
    {
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            Initializer.Initialize();
        }

        [TestMethod]
        public void BulkInserter_PersistsCorrectly()
        {
            var sourceWidgets = GetSourceWidgets();

            var inserter = new SqlBulkInserter();

            inserter.Insert(sourceWidgets);

            var savedWidgets = Db.GetResultSet("SELECT * FROM Widget;").Map<Widget>().ToArray();

            foreach (var w in savedWidgets)
            {
                Trace.WriteLine(w.ToDiagnosticString());
            }

            Assert.AreEqual(sourceWidgets.Length, savedWidgets.Length);
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
        }
    }
}
