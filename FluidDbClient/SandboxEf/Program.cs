using System;
using System.Linq;
using FluidDbClient;
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

            var data = widgets.ToStructuredData(new NewWidgetTableTypeMap());

            Db.Execute(AddScript, new {data});

            widgets = Db.GetResultSet("SELECT * FROM Widget;").Map<Widget>().ToArray();

            var firstWidget = widgets[0];

            firstWidget.Cost += 5;

            data = widgets.ToStructuredData(new UpdatedWidgetTableTypeMap());

            Db.Execute(UpdateScript, new {data});

            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        private const string AddScript =
@"
INSERT INTO Widget (ExternalId,IsArchived,[Name],Cost,CreatedTimestamp,ReleaseDate,Weight,Rating,Serialcode) 
SELECT ExternalId,0,[Name],Cost,CreatedTimestamp,ReleaseDate,Weight,Rating,SerialCode 
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
