using System;
using System.Collections.Generic;

namespace OsuAnalyzer
{
    public delegate bool Pred<T>(T x);

    public static class Util
    {
        //expects predicate which is true for upper x
        //finds the first index for which predicate is true
        public static int LowerBound<T>(List<T> list, Pred<T> pred)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            int lo = 0, hi = list.Count ;
            while (lo < hi)
            {
                int m = (hi + lo) / 2;  
                if (!pred(list[m])) lo = m + 1;
                else hi = m;
            }
            return lo;
        }
    }
}
