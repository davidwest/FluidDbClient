using System.Collections;

namespace FluidDbClient.Sql.Test.Entities
{
    public class WidgetIdentityAndValueComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((Widget) x).HasTheSameIdentityAndValue((Widget) y) ? 0 : 1;
        }
    }

    public class WidgetValueComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((Widget)x).HasTheSameValue((Widget)y) ? 0 : 1;
        }
    }
}
