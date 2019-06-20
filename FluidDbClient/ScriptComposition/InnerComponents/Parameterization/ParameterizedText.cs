using System.Collections.Generic;

namespace FluidDbClient.ScriptComposition
{    
    public class ParameterizedText<TControl> where TControl : ParameterControlBase
    {
	    internal ParameterizedText(string renderedText, IReadOnlyCollection<Parameter<TControl>> parameters)
	    {
	        Text = renderedText;
	        Parameters = parameters;
	    }
        
        public string Text { get; }
        public IReadOnlyCollection<Parameter<TControl>> Parameters { get; }
    }
}
