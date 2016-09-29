

using System.Diagnostics;

namespace FluidDbClient.Sandbox
{
    public static class Initializer
    {
        public static void Initialize()
        {
            const string  connString = "Server = localhost; Initial Catalog = Acme; Trusted_Connection = true;";
            
            var acmeDb = new AcmeDb(connString, msg => Debug.WriteLine(msg));

            DbRegistry.Initialize(acmeDb);
        }
    }
}
