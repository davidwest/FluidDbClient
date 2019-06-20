using System;
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
            
            EnsureNothingIsRegistered();
            EnsureTableTypeUniqueness(maps);

            var script = GetScriptFor(maps);

            Db.Execute(script);

            _maps.AddRange(maps);
        }
        
        public static void Register<TDatabase>(params TableTypeMap[] maps) where TDatabase : SqlDatabase
        {
            if (maps.Length == 0) return;

            EnsureNothingIsRegistered();
            EnsureTableTypeUniqueness(maps);

            var script = GetScriptFor(maps);

            Db<TDatabase>.Execute(script);

            _maps.AddRange(maps);
        }

        /// <summary>
        /// Gets TableTypeMap of type T; useable only when there is a single map registered for T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TableTypeMap<T> GetMap<T>() where T: class
        {
            return _maps.OfType<TableTypeMap<T>>().SingleOrDefault();
        }
        
        public static TableTypeMap GetMap(string typeName)
        {
            return _maps.SingleOrDefault(m => m.TypeName.Equals(typeName, StringComparison.OrdinalIgnoreCase));
        }

        private static void EnsureNothingIsRegistered()
        {
            if (_maps.Count > 0)
            {
                throw new InvalidOperationException("Table types have already been registered");
            }
        }

        private static void EnsureTableTypeUniqueness(TableTypeMap[] maps)
        {
            var typeNameGroups =
                maps
                    .Select(m => m.TypeName)
                    .ToLookup(x => x, StringComparer.OrdinalIgnoreCase);

            var duplicateTypeName = typeNameGroups.FirstOrDefault(grp => grp.Count() > 1)?.Key;

            if (duplicateTypeName != null)
            {
                throw new ArgumentException($"Duplicate table type name specified: \"{duplicateTypeName}\"", nameof(maps));
            }
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
