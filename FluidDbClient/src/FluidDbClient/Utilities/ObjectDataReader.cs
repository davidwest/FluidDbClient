//===============================================================================
// Copyright © 2008 Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
//
//This code lives here: http://code.msdn.microsoft.com/LinqObjectDataReader
//Please visit for comments, issues and updates
//

// Adapted for FluidDbClient with the following changes:
// * allow enums
// * rewrite type in Attribute to be primitive schema type (enums, nullables get simplified)
// * cleaned up

// ReSharper disable StaticMemberInGenericType
// ReSharper disable AccessToModifiedClosure

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluidDbClient
{
    /// <inheritdoc />
    /// <summary>
    /// The ObjectDataReader wraps a collection of CLR objects in a DbDataReader.  
    /// Only "scalar" properties are projected.
    /// This is useful for doing high-speed data loads with SqlBulkCopy, and copying collections
    /// of entities ot a DataTable for use with SQL Server Table-Valued parameters, or for interop
    /// with older ADO.NET applciations.
    /// For explicit control over the fields projected by the DataReader, just wrap your collection
    /// of entities in a anonymous type projection before wrapping it in an ObjectDataReader.
    /// Instead of 
    /// IEnumerable(Order) orders;
    /// ...
    /// IDataReader dr = orders.AsDataReader();
    /// do
    /// IEnumerable(Order) orders;
    /// ...
    /// var q = from o in orders
    ///         select new 
    ///         {
    ///            ID=o.ID,
    ///            ShipDate=o.ShipDate,
    ///            ProductName=o.Product.Name,
    ///            ...
    ///         }
    /// IDataReader dr = q.AsDataReader();
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class ObjectDataReader<T> : DbDataReader
    {
        private static readonly HashSet<Type> scalarTypes = new HashSet<Type>()
        {
            //reference types
            typeof(string),
            typeof(byte[]),

            //value types
            typeof(byte),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(Guid),
            typeof(bool),
            typeof(TimeSpan),
        };

        //basic list of scalar attributes for T
        private static readonly IReadOnlyList<Attribute> attributes = DiscoverScalarAttributes();

        private readonly IEnumerator<T> enumerator;

        private T current;
        private bool closed;
        
        #region Attribute inner type

        /// <summary>
        /// The Attribute is responsible for projecting a single property into a DataReader column.
        /// </summary>
        private class Attribute
        {
            //PropertyInfo propertyInfo;
            public readonly Type Type;
            public readonly string Name;

            private readonly Func<T, object> ValueAccessor;

            /// <summary>
            /// Uses Lamda expressions to create a Func(T,object) that invokes the given property getter.
            /// The property value will be extracted and cast to type TProperty
            /// </summary>
            /// <typeparam name="TObject">The type of the object declaring the property.</typeparam>
            /// <typeparam name="TProperty">The type to cast the property value to</typeparam>
            /// <param name="pi">PropertyInfo pointing to the property to wrap</param>
            /// <returns></returns>
            private static Func<TObject, TProperty> MakePropertyAccessor<TObject, TProperty>(PropertyInfo pi)
            {
                var objParam = Expression.Parameter(typeof(TObject), "obj");
                var typedAccessor = Expression.PropertyOrField(objParam, pi.Name);
                var castToObject = Expression.Convert(typedAccessor, typeof(object));
                LambdaExpression lambdaExpr = Expression.Lambda<Func<TObject, TProperty>>(castToObject, objParam);

                return (Func<TObject, TProperty>) lambdaExpr.Compile();
            }

            public Attribute(PropertyInfo pi)
            {
                Name = pi.Name;
                Type = pi.PropertyType.GetUnderlyingScalarFieldType();
                ValueAccessor = MakePropertyAccessor<T, object>(pi);
            }

            public Attribute(string name, Type type, Func<T, object> getValue)
            {
                Name = name;
                Type = type.GetUnderlyingScalarFieldType();
                ValueAccessor = getValue;
            }

            public object GetValue(T target)
            {
                return ValueAccessor(target);
            }
        }

        #endregion

        public ObjectDataReader(IEnumerable<T> sequence)
        {
            enumerator = sequence.GetEnumerator();
        }

        #region Utility Methods

        private static readonly Type nullable_T = typeof(int?).GetGenericTypeDefinition();

        private static bool IsNullable(Type t)
        {
            return t.IsGenericType &&
                   t.GetGenericTypeDefinition() == nullable_T;
        }

        private static Type StripNullableType(Type t)
        {
            return t.GetGenericArguments()[0];
        }

        #endregion

        #region GetSchemaTable

        private const string shemaTableSchema = 
@"<?xml version=""1.0"" standalone=""yes""?>
<xs:schema id=""NewDataSet"" xmlns="""" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
  <xs:element name=""NewDataSet"" msdata:IsDataSet=""true"" msdata:MainDataTable=""SchemaTable"" msdata:Locale="""">
    <xs:complexType>
      <xs:choice minOccurs=""0"" maxOccurs=""unbounded"">
        <xs:element name=""SchemaTable"" msdata:Locale="""" msdata:MinimumCapacity=""1"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""ColumnName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""ColumnOrdinal"" msdata:ReadOnly=""true"" type=""xs:int"" default=""0"" minOccurs=""0"" />
              <xs:element name=""ColumnSize"" msdata:ReadOnly=""true"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""NumericPrecision"" msdata:ReadOnly=""true"" type=""xs:short"" minOccurs=""0"" />
              <xs:element name=""NumericScale"" msdata:ReadOnly=""true"" type=""xs:short"" minOccurs=""0"" />
              <xs:element name=""IsUnique"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsKey"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""BaseServerName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""BaseCatalogName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""BaseColumnName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""BaseSchemaName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""BaseTableName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""DataType"" msdata:DataType=""System.Type, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""AllowDBNull"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""ProviderType"" msdata:ReadOnly=""true"" type=""xs:int"" minOccurs=""0"" />
              <xs:element name=""IsAliased"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsExpression"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsIdentity"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsAutoIncrement"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsRowVersion"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsHidden"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""IsLong"" msdata:ReadOnly=""true"" type=""xs:boolean"" default=""false"" minOccurs=""0"" />
              <xs:element name=""IsReadOnly"" msdata:ReadOnly=""true"" type=""xs:boolean"" minOccurs=""0"" />
              <xs:element name=""ProviderSpecificDataType"" msdata:DataType=""System.Type, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""DataTypeName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""XmlSchemaCollectionDatabase"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""XmlSchemaCollectionOwningSchema"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""XmlSchemaCollectionName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""UdtAssemblyQualifiedName"" msdata:ReadOnly=""true"" type=""xs:string"" minOccurs=""0"" />
              <xs:element name=""NonVersionedProviderType"" msdata:ReadOnly=""true"" type=""xs:int"" minOccurs=""0"" />
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:choice>
    </xs:complexType>
  </xs:element>
</xs:schema>";

        public override DataTable GetSchemaTable()
        {
            var s = new DataSet
            {
                Locale = System.Globalization.CultureInfo.CurrentCulture
            };

            s.ReadXmlSchema(new System.IO.StringReader(shemaTableSchema));

            var t = s.Tables[0];

            for (var i = 0; i < FieldCount; i++)
            {
                var row = t.NewRow();
                row["ColumnName"] = GetName(i);
                row["ColumnOrdinal"] = i;
                row["DataType"] = GetFieldType(i);
                row["DataTypeName"] = GetDataTypeName(i);
                row["ColumnSize"] = -1;
                t.Rows.Add(row);
            }

            return t;
        }

        #endregion

        #region IDataReader Members

        public override void Close()
        {
            closed = true;
        }

        public override int Depth => 1;

        public override bool IsClosed => closed;

        public override bool NextResult()
        {
            return false;
        }

        public override bool Read()
        {
            var rv = enumerator.MoveNext();

            if (rv)
            {
                current = enumerator.Current;
            }

            return rv;
        }

        public override int RecordsAffected => -1;

        #endregion

        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {
            Close();
            base.Dispose(disposing);
        }

        #endregion

        #region IDataRecord Members

        public override int FieldCount => attributes.Count;

        private TField GetValue<TField>(int i)
        {
            var val = (TField) attributes[i].GetValue(current);
            return val;
        }

        public override bool GetBoolean(int i)
        {
            return GetValue<bool>(i);
        }

        public override byte GetByte(int i)
        {
            return GetValue<byte>(i);
        }

        public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            var buf = GetValue<byte[]>(i);
            var bytes = Math.Min(length, buf.Length - (int) fieldOffset);
            Buffer.BlockCopy(buf, (int) fieldOffset, buffer, bufferoffset, bytes);

            return bytes;
        }

        public override char GetChar(int i)
        {
            return GetValue<char>(i);
        }

        public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            var s = GetValue<string>(i);
            var chars = Math.Min(length, s.Length - (int) fieldoffset);
            s.CopyTo((int) fieldoffset, buffer ?? throw new ArgumentNullException(nameof(buffer)), bufferoffset, chars);

            return chars;
        }

        public override string GetDataTypeName(int i)
        {
            return attributes[i].Type.Name;
        }

        public override DateTime GetDateTime(int i)
        {
            return GetValue<DateTime>(i);
        }

        public override decimal GetDecimal(int i)
        {
            return GetValue<decimal>(i);
        }

        public override double GetDouble(int i)
        {
            return GetValue<double>(i);
        }

        public override Type GetFieldType(int i)
        {
            var t = attributes[i].Type;

            // TODO: could remove; redundant
            if (IsNullable(t))
            {
                t = StripNullableType(t);
            }

            return t;
        }

        public override float GetFloat(int i)
        {
            return GetValue<float>(i);
        }

        public override Guid GetGuid(int i)
        {
            return GetValue<Guid>(i);
        }

        public override short GetInt16(int i)
        {
            return GetValue<short>(i);
        }

        public override int GetInt32(int i)
        {
            return GetValue<int>(i);
        }

        public override long GetInt64(int i)
        {
            return GetValue<long>(i);
        }

        public override string GetName(int i)
        {
            var a = attributes[i];
            return a.Name;
        }

        public override int GetOrdinal(string name)
        {
            for (var i = 0; i < attributes.Count; i++)
            {
                var a = attributes[i];

                if (a.Name == name)
                {
                    return i;
                }
            }

            return -1;
        }

        public override string GetString(int i)
        {
            return GetValue<string>(i);
        }
        
        public override int GetValues(object[] values)
        {
            for (var i = 0; i < attributes.Count; i++)
            {
                values[i] = GetValue(i);
            }

            return attributes.Count;
        }

        public override object GetValue(int i)
        {
            var o = GetValue<object>(i);

            return o ?? DBNull.Value;
        }

        public override bool IsDBNull(int i)
        {
            var o = GetValue<object>(i);
            return o == null;
        }

        public override object this[string name] => GetValue(GetOrdinal(name));

        public override object this[int i] => GetValue(i);

        #endregion

        #region DbDataReader Members
        
        public override System.Collections.IEnumerator GetEnumerator()
        {
            return enumerator;
        }

        public override bool HasRows => throw new NotSupportedException();

        #endregion

        private static bool IsScalarType(Type t)
        {
            return scalarTypes.Contains(t.GetUnderlyingScalarFieldType());
        }

        private static IReadOnlyList<Attribute> DiscoverScalarAttributes()
        {
            var type = typeof(T);

            //Not a collection of entities, just an IEnumerable<String> or other scalar type.
            //So add just a single Attribute that returns the object itself

            if (IsScalarType(type))
            {
                return new List<Attribute> { new Attribute("Value", type, t => t) };
            }

            //find all the scalar properties
            var allProperties =
                type.GetProperties()
                    .Where(p => IsScalarType(p.PropertyType))
                    .ToList();

            //Look for a constructor with arguments that match the properties on name and type
            //(name modulo case, which varies between constructor args and properties in coding convention)
            //If such an "ordering constructor" exists, return the properties ordered by the corresponding
            //constructor args ordinal position.  
            //An important instance of an ordering constructor, is that C# anonymous types all have one.  So
            //this enables a simple convention to specify the order of columns projected by the ObjectDataReader
            //by simply building the ObjectDataReader from an anonymous type projection.
            //If such a constructor is found, replace allProperties with a collection of properties sorted by constructor order.
            foreach (var completeConstructor in from ci in type.GetConstructors()
                                                where ci.GetParameters().Length == allProperties.Count
                                                select ci)
            {
                var q =
                    (from cp in completeConstructor.GetParameters()
                     join p in allProperties
                         on new { n = cp.Name.ToLower(), t = cp.ParameterType } equals new
                         { n = p.Name.ToLower(), t = p.PropertyType }
                     select new { cp, p }).ToList();

                if (q.Count != allProperties.Count) continue;

                //sort all properties by constructor ordinal position
                allProperties =
                    (from o in q
                     orderby o.cp.Position
                     select o.p)
                    .ToList();

                break; //stop looking for an ordering constructor
            }

            return
                allProperties
                    .Select(p => new Attribute(p))
                    .ToArray();
        }
    }
}