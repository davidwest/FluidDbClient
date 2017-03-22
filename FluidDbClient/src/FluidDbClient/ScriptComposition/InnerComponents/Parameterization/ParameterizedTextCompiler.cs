using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluidDbClient.ScriptComposition
{
    public enum ParamTextCompilerOption
    {
        AppendLine,
        Append
    }

    public class ParameterizedTextCompiler<TControl> where TControl : ParameterControlBase
    {
        private readonly List<KeyValuePair<string, object[]>> _textValuePairs;
        private readonly TextParameterizerBase<TControl> _parameterizer;

        public ParameterizedTextCompiler(TextParameterizerBase<TControl> parameterizer)
        {
            _parameterizer = parameterizer;
            _textValuePairs = new List<KeyValuePair<string, object[]>>();
        }

        public ParameterizedTextCompiler<TControl> Append(string text, params object[] values)
        {
            _textValuePairs.Add(new KeyValuePair<string, object[]>(text, values));

            return this;
        }
        
        public ParameterizedText<TControl> Compile(ParamTextCompilerOption renderOption = ParamTextCompilerOption.AppendLine)
        {
            var builder = new StringBuilder();
            var parameters = new List<Parameter<TControl>>();

            var startingIndex = 0;
            foreach (var pair in _textValuePairs)
            {
                var result = _parameterizer.Parameterize(startingIndex, pair.Key, pair.Value);

                startingIndex += result.Parameters.Count;

                parameters.AddRange(result.Parameters.SelectMany(p => p.EffectiveParameters));

                if (renderOption == ParamTextCompilerOption.AppendLine)
                {
                    builder.AppendLine(result.Text);
                }
                else
                {
                    builder.Append(result.Text);
                }
            }

            _textValuePairs.Clear();

            return new ParameterizedText<TControl>(builder.ToString(), parameters);
        }
    }
}
