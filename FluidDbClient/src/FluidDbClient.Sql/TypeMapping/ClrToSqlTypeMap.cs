
using System;
using System.Collections.Generic;
using System.Data;

namespace FluidDbClient.Sql
{
    public static class PrimitiveClrToSqlTypeMap
    {
        private static readonly Dictionary<Type, SqlDbType> Map = new Dictionary<Type, SqlDbType>
        {
            {typeof(bool), SqlDbType.Bit },
            {typeof(byte), SqlDbType.TinyInt },
            {typeof(char), SqlDbType.VarChar },
            {typeof(short), SqlDbType.SmallInt },
            {typeof(int), SqlDbType.Int },
            {typeof(long), SqlDbType.BigInt },
            {typeof(decimal), SqlDbType.Decimal },
            {typeof(double), SqlDbType.Float },
            {typeof(float), SqlDbType.Real },
            {typeof(Guid), SqlDbType.UniqueIdentifier },
            {typeof(DateTime), SqlDbType.DateTime },
            {typeof(DateTimeOffset), SqlDbType.DateTimeOffset },
            {typeof(string), SqlDbType.NVarChar },
            {typeof(char[]), SqlDbType.NVarChar },
            {typeof(byte[]), SqlDbType.Binary }
        };

        public static SqlDbType? GetSqlTypeFor(object value)
        {
            var type = value.GetType();

            return Map.ContainsKey(type) ? (SqlDbType?)Map[type] : null;
        }
    }
}
