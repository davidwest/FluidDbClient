using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
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

        [TestInitialize]
        public void Initialize()
        {
            Db.Execute("DELETE FROM Widget;");
        }

        [TestMethod]
        public void BulkInserter_PersistsCorrectly()
        {
            var sourceWidgets = GetSourceWidgets();

            var inserter = GetInserter(sourceWidgets);

            inserter.Write();
            
            BulkInserter_PersistsCorrectly(sourceWidgets);
        }

        [TestMethod]
        public void BulkInserter_PersistsCorrectly_Async()
        {
            var sourceWidgets = GetSourceWidgets();

            var inserter = GetInserter(sourceWidgets);

            inserter.WriteAsync().GetAwaiter().GetResult();
            
            BulkInserter_PersistsCorrectly(sourceWidgets);
        }

        [TestMethod]
        public void BulkInserter_WithoutInternalTransaction_AllowsPartialPersistance()
        {
            var sourceWidgets = GetSourceWidgets();
            
            var inserter = 
                GetInserter(sourceWidgets)
                    .HasNotifications(1, (src, arg) =>
                    {
                        if (arg.RowsCopied == 2)
                        {
                            throw new Exception("Failed after 2 rows copied");
                        }
                    });

            try
            {
                inserter.Write();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }

            var savedWidgets = GetSavedWidgets();

            Assert.AreEqual(2, savedWidgets.Length);
        }

        [TestMethod]
        public void BulkInserter_WithInternalTransaction_DoesNotAllowPartialPersistance()
        {
            var sourceWidgets = GetSourceWidgets();
            
            var inserter =
                GetInserter(sourceWidgets)
                    .HasOptions(SqlBulkCopyOptions.UseInternalTransaction)
                    .HasNotifications(1, (src, arg) =>
                    {
                        if (arg.RowsCopied == 2)
                        {
                            throw new Exception("Failed after 2 rows copied");
                        }
                    });

            try
            {
                inserter.Write();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }

            var savedWidgets = GetSavedWidgets();

            Assert.AreEqual(0, savedWidgets.Length);
        }

        [TestMethod]
        public void BulkInserter_UsingUncommittedTransaction_DoesNotPersistAny()
        {
            var sourceWidgets = GetSourceWidgets();
            var inserter = GetInserter(sourceWidgets);

            using (var connection = DbRegistry.GetDatabase().Provider.CreateConnection())
            {
                connection.Open();

                using (var trans = (SqlTransaction)connection.BeginTransaction())
                {
                    inserter
                        .UseTransaction(trans)
                        .Write();

                    // do not commit!
                }
            }
            
            Assert.AreEqual(0, GetSavedWidgets().Length);
        }

        [TestMethod]
        public void BulkInserter_UsingCommittedTransaction_PersistsAll()
        {
            var sourceWidgets = GetSourceWidgets();
            var inserter = GetInserter(sourceWidgets);

            using (var connection = DbRegistry.GetDatabase().Provider.CreateConnection())
            {
                connection.Open();

                using (var trans = (SqlTransaction)connection.BeginTransaction())
                {
                    inserter
                        .UseTransaction(trans)
                        .Write();

                    trans.Commit();
                }
            }

            BulkInserter_PersistsCorrectly(sourceWidgets);
        }

        [TestMethod]
        public void BulkInserter_UsingCompletedTransactionScope_PersistsAll()
        {
            var sourceWidgets = GetSourceWidgets();
            var inserter = GetInserter(sourceWidgets);

            using (var txScope = CreateTransactionScope())
            {
                inserter.Write();
                txScope.Complete();
            }

            BulkInserter_PersistsCorrectly(sourceWidgets);
        }

        [TestMethod]
        public void BulkInserter_InTransactionScope_DoesNotAllowPartialPersistance()
        {
            var sourceWidgets = GetSourceWidgets();

            var inserter =
                GetInserter(sourceWidgets)
                    .HasNotifications(1, (src, arg) =>
                    {
                        if (arg.RowsCopied == 2)
                        {
                            throw new Exception("Failed after 2 rows copied");
                        }
                    });

            try
            {
                using (var txScope = CreateTransactionScope())
                {
                    inserter.Write();
                    txScope.Complete();
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            
            var savedWidgets = GetSavedWidgets();

            Assert.AreEqual(0, savedWidgets.Length);
        }
        
        private static void BulkInserter_PersistsCorrectly(IReadOnlyCollection<Widget> sourceWidgets)
        {
            var savedWidgets = GetSavedWidgets();

            foreach (var w in savedWidgets)
            {
                Trace.WriteLine(w.ToDiagnosticString());
            }

            Assert.AreEqual(sourceWidgets.Count, savedWidgets.Length);
        }

        private static Widget[] GetSavedWidgets()
        {
            return Db.GetResultSet("SELECT * FROM Widget").Map<Widget>().ToArray();
        }

        private SqlBulkInserter GetInserter(IEnumerable<Widget> widgets, SqlBulkCopyOptions options = SqlBulkCopyOptions.Default)
        {
            var inserter =
                new SqlBulkInserter()
                    .GoesToTable(nameof(Widget))
                    .FromSource(widgets)
                    .HasOptions(options)
                    .HasNotifications(1, (src,args) => Trace.WriteLine("Wrote one!"));

            return inserter;
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

        private static TransactionScope CreateTransactionScope()
        {
            return new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });
        }
    }
}
