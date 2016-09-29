
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FluidDbClient.Sandbox.Demos
{
    public static class AsyncShell
    {
        public static async void Start(Func<Task> doIt)
        {
            try
            {
                Console.WriteLine("--- Before async call ---");

                var sw = new Stopwatch();
                sw.Start();

                await doIt();

                sw.Stop();
                Console.WriteLine($"--- After async call : {sw.Elapsed} ---");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
