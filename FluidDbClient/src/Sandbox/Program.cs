

using System;
using System.Diagnostics;

namespace FluidDbClient.Sandbox
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("*** APP INITIALIZING ***");

            Initializer.Initialize();

            Console.WriteLine("*** MAIN STARTING ***");

            
                var sw = new Stopwatch();
                sw.Start();
                DemoRunner.Start();
                sw.Stop();
                Console.WriteLine($"*** MAIN FINISHED : {sw.Elapsed} ***");


            Console.ReadKey();
        }
    }
}
