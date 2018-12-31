using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.SqlServer.Server;

namespace FluidDbClient.Sql
{
    public abstract class TableTypeMap
    {
        protected Dictionary<string, ColumnDefinition> PropertyMap { get; set; }
        internal string TypeName { get; set; }
        
        public TableTypeDefinition GetDefinition()
        {
            var normalizedColumnDefs = GetNormalizedColumnDefinitions();

            return new TableTypeDefinition(TypeName, normalizedColumnDefs);
        }

        private ColumnDefinition[] GetNormalizedColumnDefinitions()
        {
            return 
                PropertyMap.Values
                    .Where(cd => !cd.IsIgnored)
                    .Select((cd, i) => new ColumnDefinition(GetNormalizedSqlMetaData(cd.MetaData, i), cd.Behavior))
                    .ToArray();
        }

        private static SqlMetaData GetNormalizedSqlMetaData(SqlMetaData metaData, int order)
        {
            return metaData.SqlDbType.CanSpecifyPrecision() 
                ? SqlMetaDataFactory.CreateSqlMetaData(metaData.Name, metaData.SqlDbType, metaData.Precision, metaData.Scale, metaData.IsUniqueKey, order)
                : SqlMetaDataFactory.CreateSqlMetaData(metaData.Name, metaData.SqlDbType, metaData.MaxLength, metaData.IsUniqueKey, order);
        }
    }

    public abstract class TableTypeMap<T> : TableTypeMap where T : class
    {
        protected TableTypeMap()
        {
            TypeName = typeof(T).Name;

            // Default PropertyMap:

            PropertyMap =
                typeof(T).GetProperties()
                .Select(p => new
                {
                    p.Name,
                    p.PropertyType,
                    SqlType = DefaultClrToSqlTypeMap.GetSqlTypeFor(p.PropertyType)
                })
                .Where(p => p.SqlType.HasValue)
                .Select((p,i) => new
                {
                    Type = p.PropertyType,
                    MetaData = SqlMetaDataFactory.CreateSqlMetaData(p.Name, p.SqlType.Value, i)
                })
                .Select(p => new ColumnDefinition(p.MetaData, p.Type.IsNullableType() ? ColumnBehavior.Nullable : ColumnBehavior.NotNullable))
                .ToDictionary(cd => cd.MetaData.Name, cd => cd);
        }
        
        protected void HasName(string name)
        {
            TypeName = name;
        }

        protected TableTypePropertyConfiguration Property(Expression<Func<T, object>> expression)
        {
            var propertyName = expression.GetPropertyName();

            if (!PropertyMap.TryGetValue(propertyName, out var columnDef))
            {
                throw new ArgumentException($"Expression must represent property of type {typeof(T).Name}", nameof(expression));
            }

            return new TableTypePropertyConfiguration(propertyName, columnDef, (name, def) => PropertyMap[name] = def);
        }
    }
}
