using System;
using System.Linq;
using FluidDbClient.Shell;
using FluidDbClient.Sql;
using SandboxEf.Entities;
using SandboxEf.TableTypes;

namespace SandboxEf
{
    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Initializing...");
            Initializer.Initialize();

            Console.WriteLine("Starting...");

            var widgets = new[]
            {
                new Widget
                {
                    ExternalId = Guid.NewGuid(),
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
                    Name = "Dicer",
                    Cost = (decimal)55.80,
                    CreatedTimestamp = DateTimeOffset.UtcNow,
                    ReleaseDate = DateTime.UtcNow.AddDays(5).Date,
                    Weight = 5.554,
                    SerialCode = new byte[]{1, 2, 33, 53, 249}
                }
            };

            var data = widgets.OrderBy(w => w.ExternalId).ToStructuredData(new NewWidgetTableTypeMap());

            Db.Execute(Script, new {data});

            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        private const string Script =
@"
INSERT INTO Widget (ExternalId,IsArchived,[Name],Cost,CreatedTimestamp,ReleaseDate,Weight,Rating,Serialcode) 
SELECT ExternalId,0,[Name],Cost,CreatedTimestamp,ReleaseDate,Weight,Rating,SerialCode 
FROM @data;
";
    }
}
