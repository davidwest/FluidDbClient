
using System.Data;
using System.Threading.Tasks;

namespace FluidDbClient
{
    public interface IManagedDbCommand
    {
        void Execute(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        Task ExecuteAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}
