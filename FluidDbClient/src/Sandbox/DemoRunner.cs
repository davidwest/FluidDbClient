using FluidDbClient.Sandbox.Demos;
using FluidDbClient.Sandbox.Demos.Basics;
using FluidDbClient.Sandbox.Demos.SingleResultSet;
using FluidDbClient.Sandbox.Demos.ParameterSpecification;
using FluidDbClient.Sandbox.Demos.TableValuedParameters;
using FluidDbClient.Sandbox.Demos.MultipleResultSets;
using FluidDbClient.Sandbox.Demos.CommandsAndSessions;
using FluidDbClient.Sandbox.Demos.ScriptCompilation;
using FluidDbClient.Sandbox.Demos.Async;
using FluidDbClient.Sandbox.Demos.ModelMapping;

namespace FluidDbClient.Sandbox
{
    public static class DemoRunner
    {
        public static void Start()
        {
            InitializeData.Start();   // <-- only invoke if you want to populate db with initial data

            // --- Basics ---
            //DemoGetScalar.Start();
            //DemoGetSingleRecord.Start();

            // --- Single Result Set ---
            //DemoGetResultSet.Start();
            //DemoGetResultSetBuffered.Start();

            // --- Parameter Specification ---
            //DemoNullParameters1.Start();
            //DemoNullParameters2.Start();
            //DemoNullParameters3.Start();
            //DemoOutputParameters.Start();
            //DemoReturnParameters.Start();

            // --- Table Valued Parameters (Sql Server) ---
            //DemoTableValuedParameters1.Start();
            //DemoTableValuedParameters2.Start();
            DemoTableTypeMap.Start();

            // --- Multiple Result Sets ---
            //DemoProcessResultSets1.Start();
            //DemoProcessResultSets2.Start();
            //DemoProcessResultSets3.Start();
            //DemoCollectResultSets1.Start();
            //DemoCollectResultSets2.Start();

            // --- Commands & Sessions ---
            //DemoCommand.Start();
            //DemoSession.Start();
            //DemoExternalTransaction.Start();

            // --- Script Compilation ---
            //DemoScriptCompilationSimple.Start();
            //DemoScriptCompilationObnoxious.Start();
            //DemoWorkingScriptCompilation.Start();

            // --- Model Mapping ---
            //DemoModelMapping.Start();

            // ***** ASYNC *****

            //AsyncShell.Start(DemoGetScalarAsync.StartAsync);
            //AsyncShell.Start(DemoGetRecordAsync.StartAsync);

            //AsyncShell.Start(DemoCollectResultSetAsync.StartAsync);
            //AsyncShell.Start(DemoCollectResultSetsAsync.StartAsync);

            //AsyncShell.Start(DemoDbSessionAsync.StartAsync);

            //AsyncShell.Start(DemoScriptCompilationAsync.StartAsync);
        }
    }
}

