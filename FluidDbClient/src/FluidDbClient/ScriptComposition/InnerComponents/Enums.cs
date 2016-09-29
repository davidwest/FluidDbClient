

namespace FluidDbClient.ScriptComposition
{
    public enum ScriptSegmentKind
    {
        Root,
        Text,
        OpenDelimiter,
        CloseDelimiter
    }
    
    public enum ParseErrorKind
    {
        MissingOpenDelimiter,
        MissingCloseDelimiter
    }
}
