
using System.Collections.Generic;
using System.Linq;

namespace FluidDbClient.ScriptComposition
{
    internal static class InnerScriptTreeBuilder
    {
        public static ScriptNode Build(List<SourceSegment> orderedSegments)
        {
            var rootSeg = orderedSegments.First();
            orderedSegments.Remove(rootSeg);
            
            var descendantsGroupedByParent = 
                orderedSegments
                .Where(s => s.ParentOrder.HasValue)
                .ToLookup(s => s.ParentOrder.Value, s => s as ScriptSegment);

            var root = new ScriptNode(rootSeg);

            Build(root, descendantsGroupedByParent);

            return root;
        }
        
        private static void Build(ScriptNode cur, ILookup<int, ScriptSegment> parentMap)
        {
            var childSegments = parentMap[cur.Segment.Order];

            var childNodes = 
                childSegments
                .Select(seg => new ScriptNode(cur, seg))
                .ToArray();

            cur.Children = childNodes;

            foreach (var child in childNodes)
            {
                Build(child, parentMap);
            }
        }
    }
}
