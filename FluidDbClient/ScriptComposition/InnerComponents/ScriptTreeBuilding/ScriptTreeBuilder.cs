using System;
using System.Collections.Generic;
using System.Linq;
using FluidDbClient.Extensions;

namespace FluidDbClient.ScriptComposition
{
    internal static class ScriptTreeBuilder
    {
        private static readonly char[] Delimiters = {'{', '}'};

        public static ScriptNode Build(string sourceText)
        {
            var sourceSegs = GetSourceSegments(sourceText);
            AugmentSourceSegments(sourceSegs);

            var root = InnerScriptTreeBuilder.Build(sourceSegs);

            return root;
        }


        private static List<SourceSegment> GetSourceSegments(string sourceText)
        {
            sourceText = sourceText ?? string.Empty;

            var rawSegments = sourceText.ToDelimitedArray(Delimiters);

            var segments =
                rawSegments
                .Select((seg, i) =>
                {
                    var firstChar = seg[0];

                    var segKind = firstChar == Delimiters[0]
                        ? ScriptSegmentKind.OpenDelimiter
                        : firstChar == Delimiters[1]
                            ? ScriptSegmentKind.CloseDelimiter
                            : ScriptSegmentKind.Text;

                    return new SourceSegment(i + 1, segKind, seg);
                })
                .ToList();

            return segments;
        }


        private static void AugmentSourceSegments(IList<SourceSegment> segments)
        {
            var rootSeg = new SourceSegment(0, ScriptSegmentKind.Root, string.Empty);

            var parentStack = new Stack<SourceSegment>();
            parentStack.Push(rootSeg);

            for (var i = 0; i != segments.Count; i++)
            {
                var seg = segments[i];

                Action<SourceSegment> setParentId = s =>
                {
                    var parent = parentStack.Peek();
                    seg.ParentOrder = parent.Order;
                };

                switch (seg.Kind)
                {
                    case ScriptSegmentKind.OpenDelimiter:
                        setParentId(seg);
                        parentStack.Push(seg);
                        break;
                    case ScriptSegmentKind.CloseDelimiter:
                        if (parentStack.Count > 1)
                        {
                            parentStack.Pop();
                            setParentId(seg);
                        }
                        else
                        {
                            setParentId(seg);
                            seg.Error = ParseErrorKind.MissingOpenDelimiter;
                        }
                        break;
                    case ScriptSegmentKind.Text:
                        setParentId(seg);
                        break;
                }
            }

            parentStack
            .Where(seg => seg.ParentOrder.HasValue)
            .ForEach(seg => seg.Error = ParseErrorKind.MissingCloseDelimiter);

            segments.Insert(0, rootSeg);
        }
    }
}
