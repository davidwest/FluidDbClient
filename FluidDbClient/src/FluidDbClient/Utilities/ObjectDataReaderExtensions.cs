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

using System.Data.Common;
using System.Collections.Generic;
using System.Data;

namespace FluidDbClient
{    
    public static class ObjectDataReaderExtensions
    {
        /// <summary>
        /// Wraps the IEnumerable in a DbDataReader, having one column for each "scalar" property of the type T.  
        /// The collection will be enumerated as the client calls IDataReader.Read().
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DbDataReader AsDataReader<T>(this IEnumerable<T> collection)
        {
            return new ObjectDataReader<T>(collection);
        }
        
        /// <summary>
        /// Enumerates the collection and copies the data into a DataTable.
        /// </summary>
        /// <typeparam name="T">The element type of the collection.</typeparam>
        /// <param name="collection">The collection to copy to a DataTable</param>
        /// <param name="tableName"></param>
        /// <returns>A DataTable containing the scalar projection of the collection.</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection, string tableName = null)
        {
            tableName = tableName ?? typeof(T).Name;

            var t = new DataTable
            {
                Locale = System.Globalization.CultureInfo.CurrentCulture,
                TableName = tableName
            };

            var dr = new ObjectDataReader<T>(collection);

            t.Load(dr);

            return t;
        }
    }
}