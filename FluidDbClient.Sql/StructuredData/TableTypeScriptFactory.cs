using System.Collections.Generic;
using System.Linq;

namespace FluidDbClient.Sql
{
    public static class TableTypeScriptFactory
    {
        public static string CreateScriptFor(TableTypeMap map)
        {
            return CreateScriptFor(map.GetDefinition());
        }

        public static string CreateScriptFor(TableTypeDefinition def)
        {
            var effectiveColumns = 
                def.Columns
                .Where(c => !c.IsIgnored)
                .ToArray();

            var tableBody = CreateTableBody(effectiveColumns);

            var script = string.Format(ScriptTemplate, def.TypeName, tableBody);

            return script;
        }
        
        private static string CreateTableBody(IReadOnlyCollection<ColumnDefinition> columns)
        {
            var lines = columns.Select(c => CreateColumn(c)).ToList();

            var primaryKeyColumnNames =
                columns
                .Where(c => c.Behavior == ColumnBehavior.PrimaryKeyComponent)
                .Select(c => c.MetaData.Name)
                .ToArray();

            if (primaryKeyColumnNames.Length != 0)
            {
                lines.Add(CreatePrimaryKey(primaryKeyColumnNames));
            }

            var uniqueConstraintColumnNames = 
                columns
                .Where(c => c.Behavior == ColumnBehavior.UniqueKeyComponent)
                .Select(c => c.MetaData.Name)
                .ToArray();

            if (uniqueConstraintColumnNames.Length != 0)
            {
                lines.Add(CreateUniqueConstraint(uniqueConstraintColumnNames));
            }

            var body = string.Join(",\n", lines);

            return body;
        }

        private static string CreateColumn(ColumnDefinition def)
        {
            var meta = def.MetaData;
            var qualifier = def.Behavior == ColumnBehavior.Nullable ? "NULL" : "NOT NULL";

            return $"{Wrap(meta.Name)} {CreateSqlType(meta)} {qualifier}";
        }

        private static string CreateSqlType(Microsoft.Data.SqlClient.Server.SqlMetaData meta)
        {
            var sqlType = meta.SqlDbType;

            var sqlTypeStr = Wrap(sqlType.ToString());

            if (sqlType.CanSpecifyLength())
            {
                sqlTypeStr += meta.MaxLength == -1 ? "(MAX)" : $"({meta.MaxLength})";
            }

            if (sqlType.CanSpecifyPrecision())
            {
                sqlTypeStr += $"({meta.Precision},{meta.Scale})";
            }

            return sqlTypeStr;
        }

        private static string CreatePrimaryKey(IEnumerable<string> columnNames)
        {
            return $"PRIMARY KEY ({string.Join(",", columnNames.Select(Wrap))})";
        }

        private static string CreateUniqueConstraint(IEnumerable<string> columnNames)
        {
            return $"UNIQUE ({string.Join(",", columnNames.Select(Wrap))})";
        }

        private static string Wrap(string name)
        {
            return $"[{name}]";
        }
        
        private const string ScriptTemplate =
@"
IF EXISTS
(SELECT * FROM sys.types WHERE is_table_type = 1 AND name ='{0}')
DROP TYPE [{0}];

CREATE TYPE [dbo].[{0}] AS TABLE
(
{1}
);

";
    }
}
