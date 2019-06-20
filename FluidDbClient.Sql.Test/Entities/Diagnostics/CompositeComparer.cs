using System.Collections;

namespace FluidDbClient.Sql.Test.Entities
{
    public class CompositeIdentityAndValueComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((Composite)x).HasTheSameIdentityAndValue((Composite) y) ? 0 : 1;
        }
    }

    public class CompositeValueComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((Composite)x).HasTheSameValue((Composite)y) ? 0 : 1;
        }
    }
}
