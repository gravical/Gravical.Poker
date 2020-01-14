using System.Collections.Generic;

namespace Gravical.Poker.Core
{
    public class CardComparer : IEqualityComparer<Card>
    {
        public bool Equals(Card x, Card y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Card obj)
        {
            return obj.GetHashCode();
        }
    }
}