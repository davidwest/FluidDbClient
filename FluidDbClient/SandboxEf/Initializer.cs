using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using FluidDbClient;
using FluidDbClient.Sql;
using SandboxEf.Entities;
using SandboxEf.TableTypes;

namespace SandboxEf
{
    public static class Initializer
    {
        public static void Initialize()
        {
            Console.WriteLine("Please ensure all instances of SSMS are closed!");

            var connString = ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;
            DropDatabase(connString);
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
                if (dbContext.Set<Widget>().Any()) {}
            }
        }
    }
}
