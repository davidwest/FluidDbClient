using System.Collections;

namespace FluidDbClient.Sql.Test.Entities
{
    public class CompositeComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((Composite)x).HasTheSameValue((Composite) y) ? 0 : 1;
        }
    }
}
