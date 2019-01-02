using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using FluidDbClient.Sql.Test.Entities;
using FluidDbClient.Sql.Test.TableTypes;

namespace FluidDbClient.Sql.Test
{
    public static class Initializer
    {
        public static void Initialize()
        {
            var connString = ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;

            try
            {
                DropDatabase(connString);
            }
            catch (Exception)
            {
                Trace.WriteLine("Please kill all connections or ensure that all instances of SSMS are closed!");

                throw;
            }
            
            BuildDatabase();

            DbRegistry.Initialize(new SqlDatabase("DataContext", connString, msg => Debug.WriteLine(msg)));

            TableTypeRegistry.Register(new NewWidgetTableTypeMap(), new UpdatedWidgetTableTypeMap());
        }

        private static void DropDatabase(string connectionString)
        {
            System.Data.Entity.Database.Delete(connectionString);
        }

        private static void BuildDatabase()
        {
            using (var dbContext = new DataContext())
            {
                // forces database to build
                if (dbContext.Set<Widget>().Any()) { }
            }
        }
    }
}
