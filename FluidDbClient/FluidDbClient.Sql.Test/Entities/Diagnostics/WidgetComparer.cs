using System.Collections;

namespace FluidDbClient.Sql.Test.Entities
{
    public class WidgetComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((Widget) x).HasTheSameValue((Widget) y) ? 0 : 1;
        }
    }
}
