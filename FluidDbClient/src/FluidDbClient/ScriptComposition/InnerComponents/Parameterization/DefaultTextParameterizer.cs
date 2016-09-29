
using System;

namespace FluidDbClient.ScriptComposition
{
    public class DefaultTextParameterizer : TextParameterizerBase<ParameterControl>
    {
        public DefaultTextParameterizer(Func<int, object, string> getParameterName, 
                                        Func<object, string> formatLiteral) 
            : base(ParameterControl.TryParse, getParameterName, formatLiteral)
        { }
    }
}
