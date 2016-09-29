﻿
using System;
using System.Data;
using System.Data.SqlClient;

namespace FluidDbClient.Sql
{
    public static class SqlParameterFactory
    {
        public static SqlParameter CreateParameter(string name, object value)
        {
            var effectiveName = GetEffectiveParameterName(name);

            if (value == null)
            {
                return new SqlParameter(effectiveName, DBNull.Value);
            }
            
            var structuredData = value as StructuredData;

            if (structuredData != null)
            {
                return GetParameterFrom(effectiveName, structuredData);
            }

            var paramDef = value as SqlParamDef;

            if (paramDef != null)
            {
                return GetParameterFrom(effectiveName, paramDef);
            }

            var type = PrimitiveClrToSqlTypeMap.GetSqlTypeFor(value);

            if (type.HasValue)
            {
                return new SqlParameter(effectiveName, type.Value)
                {
                    Value = value
                };
            }

            return new SqlParameter(effectiveName, value);
        }

        
        private static string GetEffectiveParameterName(string sourceName)
        {
            return sourceName.StartsWith("@") ? sourceName : $"@{sourceName}";
        }

        private static SqlParameter GetParameterFrom(string effectiveName, StructuredData data)
        {
            var assignableValue = data.Rows.Count > 0 
                                    ? data.Rows 
                                    : null;

            return new SqlParameter(effectiveName, SqlDbType.Structured)
            {
                TypeName = data.TableTypeName,
                Value = assignableValue
            };
        }

        private static SqlParameter GetParameterFrom(string effectiveName, SqlParamDef paramDef)
        {
            var param = new SqlParameter(effectiveName, paramDef.Type)
            {
                Value = paramDef.Value
            };

            if (paramDef.Direction.HasValue)
            {
                param.Direction = paramDef.Direction.Value;
            }

            if (paramDef.Size.HasValue)
            {
                param.Size = paramDef.Size.Value;
            }

            if (paramDef.Precision.HasValue)
            {
                param.Precision = paramDef.Precision.Value;
            }

            if (paramDef.Scale.HasValue)
            {
                param.Scale = paramDef.Scale.Value;
            }

            return param;
        }
    }
}
