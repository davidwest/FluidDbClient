using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluidDbClient.Sql
{
    internal static class PropertyExtensions
    {
        public static Dictionary<string, object> GetPropertyMap(this object obj)
        {
            if (obj == null) return null;

            var type = obj.GetType();

            return GetPropertyMap(obj, type);
        }

        public static string GetPropertyName<T>(this Expression<Func<T, object>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression != null)
            {
                return ((MemberExpression)expression.Body).Member.Name;
            }

            var unaryExpression = expression.Body as UnaryExpression;

            if (unaryExpression == null)
            {
                throw new ArgumentException($"Expression must represent property of type {typeof(T).Name}", nameof(expression));
            }

            return ((MemberExpression)unaryExpression.Operand).Member.Name;
        }

        public static bool IsNullableType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static Dictionary<string, object> GetPropertyMap(this object obj, Type type)
        {
            return type.GetRuntimeProperties()
                   .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj));
        }
    }
}
