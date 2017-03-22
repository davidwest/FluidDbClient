using System;
using System.Data;

namespace FluidDbClient.Sql
{
    public class SqlParamDef
    {
        public SqlParamDef(SqlDbType type, object value = null)
        {
            Type = type;
            Value = value ?? DBNull.Value;
        }

        public SqlDbType Type { get; }
        public object Value { get; }

        public ParameterDirection? Direction { get; set; }
        public byte? Precision { get; set; }
        public byte? Scale { get; set; }
        public int? Size { get; set; }
    }
}
