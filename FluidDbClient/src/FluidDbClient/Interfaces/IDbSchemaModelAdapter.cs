using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FluidDbClient
{
    public interface IDbSchemaModelAdapter
    {
        DataTable GetSchemaValidatedData<T>(IEnumerable<T> items, string tableName = null) where T : class;
        
        Task<DataTable> GetSchemaValidatedDataAsync<T>(IEnumerable<T> items, string tableName = null) where T : class;
    }
}
