
using System.Collections.Generic;
using System.Data.Common;

namespace FluidDbClient
{
    public interface IManagedDbControl
    {
        IReadOnlyCollection<DbParameter> Parameters { get; }

        object this[string parameterName] { get; set; }

        void AddParameters(params DbParameter[] parameters);
        void AddParameters(IEnumerable<DbParameter> parameters);
    }
}

