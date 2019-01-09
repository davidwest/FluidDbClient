using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FluidDbClient
{
    public class DataRecord : IDataRecord
    {
        private class DataField
        {
            public DataField(int index, string name, object value)
            {
                Index = index;
                Name = name;
                Value = value;
            }

            public int Index { get; }
            public string Name { get; }
            public object Value { get; }
        }

        private readonly List<DataField> _dataFields = new List<DataField>();

        public DataRecord(IDataRecord record)
        {
            for (var i = 0; i != record.FieldCount; i++)
            {
                _dataFields.Add(new DataField(i, record.GetName(i), record[i]));
            }
        }

        public string GetName(int i) => _dataFields.First(df => df.Index == i).Name;

        public int GetOrdinal(string name)
        {
            return _dataFields.First(df => df.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).Index;
        }

        public Type GetFieldType(int i) => Get(i).GetType();
        public string GetDataTypeName(int i) => GetFieldType(i).Name;

        public bool IsDBNull(int i) => Get(i) is DBNull;

        public int FieldCount => _dataFields.Count;

        public object this[int i] => Get(i);

        public object this[string name]
        {
            get { return _dataFields.First(df => df.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).Value; }
        }

        public int GetValues(object[] values)
        {
            for (var i = 0; i != _dataFields.Count; i++)
            {
                values[i] = Get(i);
            }

            return _dataFields.Count;
        }

        public object GetValue(int i) => Get(i);
        public bool GetBoolean(int i) => (bool)Get(i);
        public byte GetByte(int i) => (byte)Get(i);
        public char GetChar(int i) => (char)Get(i);
        public DateTime GetDateTime(int i) => (DateTime)Get(i);
        public decimal GetDecimal(int i) => (decimal)Get(i);
        public double GetDouble(int i) => (double)Get(i);
        public float GetFloat(int i) => (float)Get(i);
        public Guid GetGuid(int i) => (Guid)Get(i);
        public short GetInt16(int i) => (short) Get(i);
        public int GetInt32(int i) => (int) Get(i);
        public long GetInt64(int i) => (long) Get(i);
        public string GetString(int i) => (string) Get(i);
        
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            var buf = (byte[])Get(i);
            var bytes = Math.Min(length, buf.Length - (int)fieldOffset);
            Buffer.BlockCopy(buf, (int)fieldOffset, buffer, bufferoffset, bytes);
            return bytes;
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            var s = (string)Get(i);
            var chars = Math.Min(length, s.Length - (int)fieldoffset);
            s.CopyTo((int)fieldoffset, buffer ?? throw new ArgumentNullException(nameof(buffer)), bufferoffset, chars);

            return chars;
        }

        public IDataReader GetData(int i)
        {
            throw new NotSupportedException("Cannot get IDataReader for buffered records");
        }
        
        private object Get(int i)
        {
            return _dataFields[i].Value;
        }
    }
}
