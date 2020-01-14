using System;
using System.Collections.Generic;

namespace Gravical.Poker.Core
{
    public static class LinqExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> act)
        {
            foreach (var item in items)
            {
                act(item);
            }
        }
    }
}