using System;
using System.Linq;
using System.Transactions;
using FluidDbClient.Shell;
using FluidDbClient.Sql.Test.Entities;
using FluidDbClient.Sql.Test.TableTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluidDbClient.Sql.Test
{
    [TestClass]
    public class TransactionTests
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
        public void Execute_PersistsCorrectly()
        {
            var sourceWidgets = GetSourceWidgets();
            
            Db.Execute(InsertScript, new { data = sourceWidgets.ToStructuredData(new NewWidgetTableTypeMap()) });
            
            var savedWidgets = GetSavedWidgets();

            CollectionAssert.AreEqual(sourceWidgets, savedWidgets, new WidgetValueComparer());
        }
        
        [TestMethod]
        public void Execute_WithinUncompletedTransactionScope_PersistsNothing()
        {
            var sourceWidgets = GetSourceWidgets();

            using (CreateTransactionScope())
            {
                Db.Execute(InsertScript, new {data = sourceWidgets.ToStructuredData(new NewWidgetTableTypeMap())});
            }

            var savedWidgets = GetSavedWidgets();

            Assert.AreEqual(0, savedWidgets.Length);
        }
        
        [TestMethod]
        public void Execute_WithinCompletedTransactionScope_PersistsCorrectly()
        {
            var sourceWidgets = GetSourceWidgets();

            using (var tx = CreateTransactionScope())
            {
                Db.Execute(InsertScript, new { data = sourceWidgets.ToStructuredData(new NewWidgetTableTypeMap()) });

                tx.Complete();
            }

            var savedWidgets = GetSavedWidgets();
            
            CollectionAssert.AreEqual(sourceWidgets, savedWidgets, new WidgetValueComparer());
        }

        [TestMethod]
        public void ExecuteWithoutTransaction_WithingCompletedTransactionScope_PersistsCorrectly()
        {
            var sourceWidgets = GetSourceWidgets();

            using (var tx = CreateTransactionScope())
            {
                Db.ExecuteWithoutTransaction(InsertScript, new
                {
                    data = sourceWidgets.ToStructuredData(new NewWidgetTableTypeMap())
                });

                tx.Complete();
            }

            var savedWidgets = GetSavedWidgets();

            CollectionAssert.AreEqual(sourceWidgets, savedWidgets, new WidgetValueComparer());
        }

        private static Widget[] GetSavedWidgets()
        {
            return Db.GetResultSet("SELECT * FROM Widget ORDER BY Name;").Map<Widget>().ToArray();
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

        private static TransactionScope CreateTransactionScope()
        {
            return new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });
        }

        private const string InsertScript =
            @"
INSERT INTO Widget (ExternalId,[Environment],IsArchived,[Name],Cost,CreatedTimestamp,ReleaseDate,Weight,Rating,Serialcode) 
SELECT ExternalId,[Environment],0,[Name],Cost,CreatedTimestamp,ReleaseDate,Weight,Rating,SerialCode 
FROM @data;
";
    }
}
