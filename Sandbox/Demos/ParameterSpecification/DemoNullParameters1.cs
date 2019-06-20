

using System;
using System.Diagnostics;

namespace FluidDbClient.Sandbox.Demos.ParameterSpecification
{
    public class DemoNullParameters1
    {
        public static void Start()
        {
            var cmd = new ScriptDbCommand("UPDATE Robot SET DateDestroyed = @dd WHERE Name = @name;", 
                                          new {name = "Terminator", dd = (DateTime?)null});

            // NOTE: the @dd parameter type is semantically incorrect due to 
            // not being able to infer the expected type given a null value...

            Debug.WriteLine(cmd.ToDiagnosticString());

            // ... but it does not prohibit the command from executing correctly
            cmd.Execute();
        }
    }
}
