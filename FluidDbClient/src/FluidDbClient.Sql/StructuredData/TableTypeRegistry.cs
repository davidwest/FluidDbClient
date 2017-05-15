using System.Collections.Generic;
using System.Text;
using FluidDbClient.Shell;

namespace FluidDbClient.Sql
{
    public static class TableTypeRegistry
    {
        public static void Register(params TableTypeMap[] maps)
        {
            if (maps.Length == 0) return;

            var script = GetScriptFor(maps);

            Db.Execute(script);
        }
        
        public static void Register<TDatabase>(params TableTypeMap[] maps) where TDatabase : SqlDatabase
        {
            if (maps.Length == 0) return;

            var script = GetScriptFor(maps);

            Db<TDatabase>.Execute(script);
        }
        
        private static string GetScriptFor(IEnumerable<TableTypeMap> maps)
        {
            var builder = new StringBuilder();

            foreach (var map in maps)
            {
                var script = TableTypeScriptFactory.CreateScriptFor(map);
                builder.AppendLine(script);
            }

            return builder.ToString();
        }
    }
}
