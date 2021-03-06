﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FluidDbClient.Sql
{
    public abstract class TableTypeMap
    {
        protected Dictionary<string, ColumnDefinition> PropertyMap { get; set; }
        internal string TypeName { get; set; }
        
        public TableTypeDefinition GetDefinition()
        {
            return new TableTypeDefinition(TypeName, PropertyMap.Values.ToArray());
        }
    }

    public abstract class TableTypeMap<T> : TableTypeMap where T : class
    {
        protected TableTypeMap()
        {
            TypeName = typeof(T).Name;

            // Start with default property map:

            PropertyMap =
                typeof(T).GetProperties()
                .Select(p => new
                {
                    p.Name,
                    p.PropertyType,
                    SqlType = DefaultClrToSqlTypeMap.GetSqlTypeFor(p.PropertyType)
                })
                .Where(p => p.SqlType.HasValue)
                .Select(p => new
                {
                    Type = p.PropertyType,
                    MetaData = SqlMetaDataFactory.CreateSqlMetaData(p.Name, p.SqlType.Value)
                })
                .Select(p => new ColumnDefinition(p.Type, p.MetaData, p.Type.IsNullableType() ? ColumnBehavior.Nullable : ColumnBehavior.NotNullable))
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
