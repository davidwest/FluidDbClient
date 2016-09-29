
using System.Collections.Generic;
using System.Linq;

namespace FluidDbClient.ScriptComposition
{    
    public class Parameter<TControl> where TControl : ParameterControlBase
    {
        internal Parameter(object value, string name, IReadOnlyCollection<TControl> controls)
        {
            Value = value;
            Name = name;
            Controls = controls;

            var propertyMap = value.GetPropertyMapIfAnonymous();

            if (propertyMap == null)
            {
                EffectiveParameterNameExpression = Name;
                EffectiveParameters = new[] {this};
                return;
            }

            EffectiveParameters =
                propertyMap
                .Select(p => new Parameter<TControl>(p.Value, Name, p.Key, Controls))
                .ToArray();

            EffectiveParameterNameExpression = EffectiveParameters.Select(p => p.Name).ToCsv();
        }

        private Parameter(object value, string namePrefix, string namePostfix, IReadOnlyCollection<TControl> controls)
        {
            Value = value;
            Name = $"{namePrefix}_{namePostfix}";
            Controls = controls;

            EffectiveParameterNameExpression = Name;
            EffectiveParameters = new[] {this};
        }


        public string Name { get; }
        public object Value { get; }
        public IReadOnlyCollection<TControl> Controls { get; }
        
        internal string EffectiveParameterNameExpression { get; }
        internal Parameter<TControl>[] EffectiveParameters { get; }
    }
}
