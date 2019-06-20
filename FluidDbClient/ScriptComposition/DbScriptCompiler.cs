using System.Linq;
using FluidDbClient.ScriptComposition;

namespace FluidDbClient
{
    public abstract class DbScriptCompilerBase
    {
        private readonly Database _database;
        private readonly ParameterizedTextCompiler<ParameterControl> _compiler;
        
        protected DbScriptCompilerBase(Database database)
        {
            _database = database;

            var parameterizer = new DefaultTextParameterizer((index, val) => _database.Provider.Interpreter.GetPrefixedParameterName($"p{index}"), 
                                                             val => _database.Provider.Interpreter.FormatScriptLiteral(val));

            _compiler = new ParameterizedTextCompiler<ParameterControl>(parameterizer);
        }

        public DbScriptDocument Compile()
        {
            var compilation = _compiler.Compile();

            var dbParameters = 
                compilation.Parameters
                .Select(p => _database.Provider.CreateParameter(p.Name, p.Value))
                .ToArray();
            
            var doc = new DbScriptDocument(compilation.Text, dbParameters);

            return doc;
        }

        protected void InnerAppend(string text, params object[] values)
        {
            _compiler.Append(text, values);
        }
    }

    public class DbScriptCompiler : DbScriptCompilerBase
    {
        public DbScriptCompiler() : base(DbRegistry.GetDatabase())
        { }

        public DbScriptCompiler Append(string text, params object[] values)
        {
            InnerAppend(text, values);
            return this;
        }
    }

    public class DbScriptCompiler<TDatabase> : DbScriptCompilerBase where TDatabase : Database
    {
        public DbScriptCompiler() : base(DbRegistry.GetDatabase<TDatabase>())
        { }

        public DbScriptCompiler<TDatabase> Append(string text, params object[] values)
        {
            InnerAppend(text, values);
            return this;
        }
    }
}


