using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniqueCollection
{
    public static class Extensions
    {
        public static bool IsUnique(this List<int> list)
        {
            if (list.Distinct().Count() == list.Count()) //if the number of unique is equal to the number of all, they are all unique.
            {
                return true;
            }
            //   Console.WriteLine($"Disctinct number: {lista.Distinct().Count()}  is different from the number of elements: {lista.Count()}");
            return false;
        }

        public static bool IsUnique(this ConcurrentBag<int> cb)
        {
            if (cb.Distinct().Count() == cb.Count()) return true;
            else return false;
        }
    }
}
