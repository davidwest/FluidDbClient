
using System.Collections.Generic;
using System.Linq;

namespace FluidDbClient.ScriptComposition
{
    public class ScriptNode
    {
        internal ScriptNode(ScriptSegment segment)
        {
            Parent = null;
            Segment = segment;
            Level = 0;
        }

        internal ScriptNode(ScriptNode parent, ScriptSegment segment)
        {
            Parent = parent;
            Segment = segment;
            Level = parent.Level + 1;
        }

        public int Level { get; }
        public ScriptSegment Segment { get; }

        public ScriptNode Parent { get; }
        public ScriptNode[] Children { get; internal set; }

        public IEnumerable<ScriptNode> SelfAndDescendants()
        {
            return RecursiveYield(this);
        }

        private static IEnumerable<ScriptNode> RecursiveYield(ScriptNode node)
        {
            yield return node;
            
            foreach (var descendant in node.Children.SelectMany(RecursiveYield))
            {
                yield return descendant;
            }
        }
    }
}
