using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FluidDbClient.Shell;

namespace FluidDbClient.Sandbox.Demos.ModelMapping
{
    public class RobotDto
    {
        private string _name = string.Empty;
        private string _description = string.Empty;

        public RobotDto()
        {
            Messages = new List<string>();
        }

        public int RobotId { get; set; }

        public string Name
        {
            get { return _name; }
            set { _name = value ?? string.Empty; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value ?? string.Empty; }
        }

        public bool IsEvil { get; set; }

        public List<string> Messages { get; set; }
    }

    public static class DemoModelMapping
    {
        public static void Start()
        {
            var robots = 
                Db.GetResultSet("SELECT * FROM Robot;")
                .Select(r => r.Map<RobotDto>())
                .ToArray();

            var builder = new StringBuilder();

            foreach (var bot in robots)
            {
                builder.AppendLine($"{bot.RobotId,-6} {bot.Name,-25} {bot.Description,-75} {bot.IsEvil}");
            }

            Debug.WriteLine(builder);
        }
    }
}
