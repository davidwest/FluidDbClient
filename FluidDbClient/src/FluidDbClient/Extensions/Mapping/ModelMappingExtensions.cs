using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluidDbClient
{
    public static class ModelMappingExtensions
    {
        private static readonly char[] PropertyDelimiters = { '_', '-'};

        public static T Map<T>(this IDataRecord r, string prefix = null) where T : class, new()
        {
            var obj = new T();
            var type = obj.GetType();

            prefix = prefix ?? type.Name;

            var namedValues =
                Enumerable.Range(0, r.FieldCount)
                .Select(i => new
                {
                    CandidateNames = GetCandidatePropertyNames(prefix, r.GetName(i)),
                    Value = r.GetValue(i).GetEffectiveValue()
                });

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var pair in namedValues)
            {
                var matchingProperty = properties.FirstOrDefault(p => pair.CandidateNames.Contains(p.Name, StringComparer.OrdinalIgnoreCase));
                
                matchingProperty?.SetValue(obj, pair.Value, null);
            }

            return obj;
        }

        public static TProp MapProperty<T, TProp>(this IDataRecord r, Expression<Func<T, TProp>> accessor) where T : class
        {
            var type = typeof(T);
            var propName = accessor.GetPropertyName();

            var namedValue =
                Enumerable.Range(0, r.FieldCount)
                    .Select(i => new
                    {
                        CandidateNames = GetCandidateComplexPropertyNames(type.Name, r.GetName(i)),
                        Value = r.GetValue(i).GetEffectiveValue()
                    })
                    .First(pair => pair.CandidateNames.Contains(propName, StringComparer.OrdinalIgnoreCase));

            return (TProp)namedValue.Value;
        }

        public static IEnumerable<T> Map<T>(this IEnumerable<IDataRecord> records, string prefix = null) where T : class, new()
        {
            return records.Select(r => r.Map<T>(prefix));
        }
        
        public static T1 Map<T1, T2>(
            this IDataRecord r,
            Func<IDataRecord, T1> map1,
            Func<IDataRecord, T2> map2,
            Action<T1, T2> combine)
        {
            var t1 = map1(r);

            combine(t1, map2(r));

            return t1;
        }

        public static TResult Map<T1, T2, TResult>(
            this IDataRecord r,
            Func<IDataRecord, T1> map1,
            Func<IDataRecord, T2> map2,
            Func<IDataRecord, T1, T2, TResult> combine)
        {
            return combine(r, map1(r), map2(r));
        }

        public static T1 Map<T1, T2>(this IDataRecord record, Action<T1, T2> combine) 
            where T1 : class, new()
            where T2 : class, new()
        {
            return record.Map(r => r.Map<T1>(), r => r.Map<T2>(), combine);
        }

        public static TResult Map<T1, T2, TResult>(
            this IDataRecord record, 
            Func<IDataRecord, T1, T2, TResult> combine)
            where T1 : class, new()
            where T2 : class, new()
        {
            return record.Map(r => r.Map<T1>(), r => r.Map<T2>(), combine);
        }

        public static IEnumerable<T1> Map<T1, T2>(this IEnumerable<IDataRecord> records, Action<T1, T2> combine)
            where T1 : class, new()
            where T2 : class, new()
        {
            return records.Select(r => r.Map(combine));
        }

        public static IEnumerable<TResult> Map<T1, T2, TResult>(
            this IEnumerable<IDataRecord> records, 
            Func<IDataRecord, T1, T2, TResult> combine)
            where T1 : class, new()
            where T2 : class, new()
        {
            return records.Select(r => r.Map(combine));
        }

        public static T1 Map<T1, T2, T3>(
            this IDataRecord r,
            Func<IDataRecord, T1> map1,
            Func<IDataRecord, T2> map2,
            Func<IDataRecord, T3> map3,
            Action<T1, T2, T3> combine)
        {
            var t1 = map1(r);

            combine(t1, map2(r), map3(r));

            return t1;
        }

        public static TResult Map<T1, T2, T3, TResult>(
            this IDataRecord r,
            Func<IDataRecord, T1> map1,
            Func<IDataRecord, T2> map2,
            Func<IDataRecord, T3> map3,
            Func<IDataRecord, T1, T2, T3, TResult> combine)
        {
            return combine(r, map1(r), map2(r), map3(r));
        }

        public static T1 Map<T1, T2, T3>(this IDataRecord record, Action<T1, T2, T3> combine)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
        {
            return record.Map(r => r.Map<T1>(), r => r.Map<T2>(), r => r.Map<T3>(), combine);
        }

        public static TResult Map<T1, T2, T3, TResult>(
            this IDataRecord record, 
            Func<IDataRecord, T1, T2, T3, TResult> combine)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
        {
            return record.Map(r => r.Map<T1>(), r => r.Map<T2>(), r => r.Map<T3>(), combine);
        }

        public static IEnumerable<T1> Map<T1, T2, T3>(this IEnumerable<IDataRecord> records, Action<T1, T2, T3> combine)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
        {
            return records.Select(r => r.Map(combine));
        }

        public static IEnumerable<TResult> Map<T1, T2, T3, TResult>(
            this IEnumerable<IDataRecord> records, 
            Func<IDataRecord, T1, T2, T3, TResult> combine)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
        {
            return records.Select(r => r.Map(combine));
        }

        public static T1 Map<T1, T2, T3, T4>(
            this IDataRecord r,
            Func<IDataRecord, T1> map1,
            Func<IDataRecord, T2> map2,
            Func<IDataRecord, T3> map3,
            Func<IDataRecord, T4> map4,
            Action<T1, T2, T3, T4> combine)
        {
            var t1 = map1(r);

            combine(t1, map2(r), map3(r), map4(r));

            return t1;
        }

        public static TResult Map<T1, T2, T3, T4, TResult>(
            this IDataRecord r,
            Func<IDataRecord, T1> map1,
            Func<IDataRecord, T2> map2,
            Func<IDataRecord, T3> map3,
            Func<IDataRecord, T4> map4,
            Func<IDataRecord, T1, T2, T3, T4, TResult> combine)
        {
            return combine(r, map1(r), map2(r), map3(r), map4(r));
        }

        public static T1 Map<T1, T2, T3, T4>(this IDataRecord record, Action<T1, T2, T3, T4> combine)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
            where T4 : class, new()
        {
            return record.Map(r => r.Map<T1>(), r => r.Map<T2>(), r => r.Map<T3>(), r => r.Map<T4>(), combine);
        }

        public static TResult Map<T1, T2, T3, T4, TResult>(
            this IDataRecord record, 
            Func<IDataRecord, T1, T2, T3, T4, TResult> combine)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
            where T4 : class, new()
        {
            return record.Map(r => r.Map<T1>(), r => r.Map<T2>(), r => r.Map<T3>(), r => r.Map<T4>(), combine);
        }

        public static IEnumerable<T1> Map<T1, T2, T3, T4>(this IEnumerable<IDataRecord> records, Action<T1, T2, T3, T4> combine)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
            where T4 : class, new()
        {
            return records.Select(r => r.Map(combine));
        }

        public static IEnumerable<TResult> Map<T1, T2, T3, T4, TResult>(
            this IEnumerable<IDataRecord> records, 
            Func<IDataRecord, T1, T2, T3, T4, TResult> combine)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
            where T4 : class, new()
        {
            return records.Select(r => r.Map(combine));
        }


        private static object GetEffectiveValue(this object fieldValue)
        {
            return fieldValue is DBNull ? null : fieldValue;
        }

        private static IEnumerable<string> GetCandidatePropertyNames(string prefix, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(prefix))
            {
                yield return fieldName;
            }

            var parts = fieldName.Split(PropertyDelimiters, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 1) yield return parts[0];

            for (var i = 0; i < parts.Length - 1; i++)
            {
                var candidatePrefix = parts[i];

                if (!candidatePrefix.Equals(prefix, StringComparison.OrdinalIgnoreCase)) continue;

                var propertyName = parts[i + 1];

                yield return propertyName;
            }
        }
        
        private static IEnumerable<string> GetCandidateComplexPropertyNames(string typeName, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                yield break;
            }

            var parts = fieldName.Split(PropertyDelimiters, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 1) yield break;

            for (var i = 0; i < parts.Length - 1; i++)
            {
                var candidatePrefix = parts[i];

                if (!candidatePrefix.Equals(typeName, StringComparison.OrdinalIgnoreCase)) continue;

                var propertyName = parts[i + 1];

                yield return propertyName;
            }
        }
    }
}
