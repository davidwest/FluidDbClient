using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluidDbClient.ScriptComposition
{    
    public abstract class TextParameterizerBase<TControl> where TControl : ParameterControlBase
    {
        #region ----- Placeholder classes -----

        internal class ValuePlaceholder
        {
            public ValuePlaceholder(ScriptSegment segment, int valueIndex)
            {
                Segment = segment;
                ValueIndex = valueIndex;
            }

            public ScriptSegment Segment { get; }
            public int ValueIndex { get; }
        }
        
        internal class ParameterPlaceholder
        {
            public ParameterPlaceholder(ScriptSegment segment, TControl control)
            {
                Segment = segment;
                Control = control;
            }

            public ScriptSegment Segment { get; }
            public TControl Control { get; }
        }

        #endregion ------------------------------------------------------------------

        private const int ParamPlaceholderNestingDepth = 2;
        private const int ValuePlaceholderNestingDepth = 3;
        
        private readonly Func<string, TControl> _parseControl;
        private readonly Func<int, object, string> _getParameterName;
        private readonly Func<object, string> _formatLiteral;
        
        protected TextParameterizerBase(Func<string, TControl> parseControl, 
                                        Func<int, object, string> getParameterName, 
                                        Func<object, string> formatLiteral)
        {
            _parseControl = parseControl;
            _getParameterName = getParameterName;
            _formatLiteral = formatLiteral;
        }

        public ParameterizedText<TControl> Parameterize(string controlText, params object[] values)
        {
            return Parameterize(0, controlText, values);
        } 

        internal ParameterizedText<TControl> Parameterize(int baseIndex, string controlText, params object[] values)
        {
            var root = GetValidatedScriptTree(controlText);

            var valuePlaceholders = new List<ValuePlaceholder>();
            var paramPlaceholders = new List<ParameterPlaceholder>();

            GatherPlaceholders(root, valuePlaceholders, paramPlaceholders);
            
            Validate(valuePlaceholders, paramPlaceholders, values);

            var valueIndexParamMap = GetValueIndexParameterMap(baseIndex, paramPlaceholders, values);

            var renderedText = GetRenderedText(root, valuePlaceholders, paramPlaceholders, values, valueIndexParamMap);
               
            var result = new ParameterizedText<TControl>(renderedText, valueIndexParamMap.Values.ToArray());

            return result;
        }
        
        private string GetRenderedText(ScriptNode root, 
                                       IReadOnlyCollection<ValuePlaceholder> literalValueMappings, 
                                       IReadOnlyCollection<ParameterPlaceholder> parameterMappings, 
                                       IReadOnlyList<object> values, 
                                       IReadOnlyDictionary<int, Parameter<TControl>> valueIndexParamMap)
        {
            var builder = new StringBuilder();
            
            foreach (var node in root.SelfAndDescendants())
            {
                var segment = node.Segment;

                if (segment.Kind == ScriptSegmentKind.OpenDelimiter || 
                    segment.Kind == ScriptSegmentKind.CloseDelimiter)
                {
                    continue;
                }
                
                var literalValueMapping = literalValueMappings.FirstOrDefault(sm => sm.Segment.Order == segment.Order);

                if (literalValueMapping != null)
                {
                    var valIndex = literalValueMapping.ValueIndex;
                    var replacedValue = values[valIndex];
                    var formattedLiteral = _formatLiteral(replacedValue);
                    builder.Append(formattedLiteral);
                    continue;
                }

                var parameterMapping = parameterMappings.FirstOrDefault(sm => sm.Segment.Order == segment.Order);

                if (parameterMapping == null)
                {
                    builder.Append(segment.Value);
                    continue;
                }

                var paramIndex = parameterMapping.Control.ValueIndex;
                var parameter = valueIndexParamMap[paramIndex];

                builder.Append(parameter.EffectiveParameterNameExpression);
            }

            return builder.ToString();
        }

        
        private IReadOnlyDictionary<int, Parameter<TControl>> GetValueIndexParameterMap(int baseIndex, 
                                                                                        IEnumerable<ParameterPlaceholder> paramPlaceholders, 
                                                                                        IReadOnlyList<object> values)
        {
            var placeHoldersGroupedByValueIndex =
                paramPlaceholders
                .ToLookup(sm => sm.Control.ValueIndex);

            var partialParameters =
                (from i in Enumerable.Range(0, values.Count)
                 let placeHoldersInGroup = placeHoldersGroupedByValueIndex[i].ToArray()
                 where placeHoldersInGroup.Length > 0
                 let value = values[i]
                 let controls = placeHoldersInGroup.Select(sm => sm.Control).ToArray()
                 select new {ValueIndex = i, Value = value, Controls = controls})
                .ToArray();

            var paramNames = partialParameters.Select((p, i) => _getParameterName(baseIndex + i, p.Value)).ToArray();

            if (paramNames.Distinct().Count() != paramNames.Length)
            {
                throw new InvalidOperationException("Parameter names are not unique");
            }
            
            var map = new Dictionary<int, Parameter<TControl>>();
            
            for (var i = 0; i != partialParameters.Length; i ++)
            {
                var partialParameter = partialParameters[i];

                map.Add(partialParameter.ValueIndex, new Parameter<TControl>(partialParameter.Value, paramNames[i], partialParameter.Controls));
            }

            return map;
        }

        
        private static void Validate(IEnumerable<ValuePlaceholder> valuePlaceHolders, 
                                     IEnumerable<ParameterPlaceholder> paramPlaceholders, 
                                     IReadOnlyCollection<object> values)
        {
            var valueIndexes = GetValueIndexSequence(valuePlaceHolders, paramPlaceholders);
            
            var isValidRange = valueIndexes.SequenceEqual(Enumerable.Range(0, valueIndexes.Length));

            if (!isValidRange)
            {
                throw new FormatException($"Controls have an invalid index range: {valueIndexes.ToCsv()}");
            }

            if (values.Count != valueIndexes.Length)
            {
                throw new InvalidOperationException($"Number of values ({values.Count}) does not match the range specified in the control string ({valueIndexes.Length})");
            }
        }

        
        private void GatherPlaceholders(ScriptNode root, 
                                        ICollection<ValuePlaceholder> valuePlaceholders, 
                                        ICollection<ParameterPlaceholder> paramPlaceholders)
        {
            foreach (var node in root.SelfAndDescendants())
            {
                var seg = node.Segment;

                if (seg.Kind != ScriptSegmentKind.Text) continue;

                if (node.Level == ValuePlaceholderNestingDepth)
                {
                    var mapping = GetValuePlaceholder(seg);
                    valuePlaceholders.Add(mapping);
                }
                else if (node.Level == ParamPlaceholderNestingDepth)
                {
                    var mapping = GetParameterPlaceholder(seg);
                    paramPlaceholders.Add(mapping);
                }
            }
        }


        private static ValuePlaceholder GetValuePlaceholder(ScriptSegment segment)
        {
            if (!int.TryParse(segment.Value.Trim(), out var valueIndex))
            {
                throw new FormatException($"Could not parse value index from text : {segment.Value.WrapDoubleQuotes()}");
            }

            return new ValuePlaceholder(segment, valueIndex);
        }
        
        private ParameterPlaceholder GetParameterPlaceholder(ScriptSegment segment)
        {
            var control = _parseControl(segment.Value);

            if (control == null)
            {
                throw new FormatException($"Could not parse control from text : {segment.Value.WrapDoubleQuotes()}");
            }

            return new ParameterPlaceholder(segment, control);
        }

        
        private static ScriptNode GetValidatedScriptTree(string controlText)
        {
            var root = ScriptTreeBuilder.Build(controlText);

            foreach (var node in root.SelfAndDescendants())
            {
                if (node.Segment.Error != null)
                {
                    throw new FormatException($"Source text is improperly formatted : {node.Segment.Error}");
                }

                if (node.Segment.Kind == ScriptSegmentKind.OpenDelimiter && node.Children.Length == 0)
                {
                    throw new FormatException("One or more controls in the source text is empty");
                }

                if (node.Level > 3)
                {
                    throw new FormatException("Source text has more than two levels of nesting");
                }

                if (node.Level == 1 && node.Children.Length > 2)
                {
                    throw new FormatException("Improper placeholder formatting");
                }
            }

            return root;
        }


        private static int[] GetValueIndexSequence(IEnumerable<ValuePlaceholder> valuePlaceholders, IEnumerable<ParameterPlaceholder> paramPlaceholders)
        {
            var valueIndexes =
                paramPlaceholders
                .Select(acc => acc.Control.ValueIndex)
                .Union(valuePlaceholders.Select(m => m.ValueIndex))
                .OrderBy(i => i)
                .ToArray();

            return valueIndexes;
        }
    }
}
