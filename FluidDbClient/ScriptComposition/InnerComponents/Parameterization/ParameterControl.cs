using System;

namespace FluidDbClient.ScriptComposition
{
    public abstract class ParameterControlBase
    {
        protected ParameterControlBase(int valueIndex)
        {
            if (valueIndex < 0)
            {
                throw new InvalidOperationException();
            }

            ValueIndex = valueIndex;
        }

        public int ValueIndex { get; }
    }

    public class ParameterControl : ParameterControlBase
    {
        public ParameterControl(int valueIndex) : base(valueIndex)
        { }

        public static ParameterControl TryParse(string candidateVal)
        {
            return int.TryParse(candidateVal.Trim(), out var valueIndex)
                ? new ParameterControl(valueIndex)
                : null;
        }
    }
}
