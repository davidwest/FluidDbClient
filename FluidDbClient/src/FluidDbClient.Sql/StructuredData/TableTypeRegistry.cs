using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluidDbClient.Shell;

namespace FluidDbClient.Sql
{
    public static class TableTypeRegistry
    {
        private static readonly List<TableTypeMap> _maps = new List<TableTypeMap>();

        public static void Register(params TableTypeMap[] maps)
        {
            if (maps.Length == 0) return;

            var script = GetScriptFor(maps);

            Db.Execute(script);

            _maps.AddRange(maps);
        }
        
        public static void Register<TDatabase>(params TableTypeMap[] maps) where TDatabase : SqlDatabase
        {
            if (maps.Length == 0) return;

            var script = GetScriptFor(maps);

            Db<TDatabase>.Execute(script);

            _maps.AddRange(maps);
        }

        /// <summary>
        /// Gets TableTypeMap of type T; valid only when there is a single map registered for T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TableTypeMap<T> GetMap<T>() where T: class
        {
            return _maps.OfType<TableTypeMap<T>>().SingleOrDefault();
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
