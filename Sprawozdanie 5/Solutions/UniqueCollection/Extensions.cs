using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace UniqueCollection
{
    public static class Extensions
    {
        public static bool IsUnique(this List<int> lista)
        {
            return lista.Distinct().Count() == lista.Count();
        }

        public static bool IsUnique(this ConcurrentBag<int> cb)
        {
            return cb.Distinct().Count() == cb.Count();
        }
    }
}
