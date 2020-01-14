using System;
using System.Collections.Generic;

namespace Gravical.Poker.Core
{
    /// <summary>
    ///  HandGrouping builds list of multiple cards, pair, three of a kind, and four of a kind.
    /// </summary>
    public class HandGrouping
    {
        public List<List<Card>> FourOfAKind { get; }
        public List<List<Card>> ThreeOfAKind { get; }
        public List<List<Card>> Pair { get; }
        public List<List<Card>> Single { get; }

        public HandGrouping(Card[] cards)
        {
            Guards.ArgumentNotNull(cards, nameof(cards));
            Guards.ArgumentSuccess(cards.Length >= 5 && cards.Length <= 7, nameof(cards), "Only 5-7 cards can be grouped");

            var groups = new Dictionary<Face, List<Card>>();
            var unique = new HashSet<Card>();
            foreach (var card in cards)
            {
                if (!card.IsValid)
                    throw new ArgumentException("Invalid cards cannot be grouped", nameof(cards));
                if (unique.Contains(card))
                    throw new ArgumentException("Duplicate cards cannot be grouped", nameof(cards));
                unique.Add(card);

                if (!groups.ContainsKey(card.Face))
                    groups.Add(card.Face, new List<Card>());

                groups[card.Face].Add(card);
            }

            FourOfAKind = new List<List<Card>>();
            ThreeOfAKind = new List<List<Card>>();
            Pair = new List<List<Card>>();
            Single = new List<List<Card>>();

            foreach (var face in groups.Keys)
            {
                var list = groups[face];
                if (list.Count == 4)
                    FourOfAKind.Add(list);
                else if (list.Count == 3)
                    ThreeOfAKind.Add(list);
                else if (list.Count == 2)
                    Pair.Add(list);
                else if (list.Count == 1)
                    Single.Add(list);
                else
                    throw new InvalidOperationException("Impossible grouping");
            }
        }
    }
}