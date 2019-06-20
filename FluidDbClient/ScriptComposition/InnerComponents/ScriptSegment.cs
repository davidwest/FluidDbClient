namespace FluidDbClient.ScriptComposition
{
    public class ScriptSegment
    {
        internal ScriptSegment(int order, ScriptSegmentKind kind, string text)
        {
            Order = order;
            Kind = kind;
            Value = text;
        }

        public int Order { get; }
        public ScriptSegmentKind Kind { get; }
        public string Value { get; }

        public ParseErrorKind? Error { get; internal set; }

        public override string ToString()
        {
            var errStr = Error.HasValue ? $"*** {Error} ***" : string.Empty;

            return $"{Order,-6} {Kind,-20} : {Value,-20} {errStr}";
        }
    }

    internal class SourceSegment : ScriptSegment
    {
        internal SourceSegment(int order, ScriptSegmentKind kind, string text) : base(order, kind, text)
        { }
        
        public int? ParentOrder { get; internal set; }

        public override string ToString()
        {
            var errStr = Error.HasValue ? $"*** {Error} ***" : string.Empty;

            return $"{Order,-6} {ParentOrder,-6} {Kind,-20} : {Value,-20} {errStr}";
        }
    }
}
