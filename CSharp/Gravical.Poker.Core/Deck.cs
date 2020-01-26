using System;
using System.Collections.Generic;
using System.Linq;

namespace Gravical.Poker.Core
{
    /// <summary>
    /// A deck creates a shuffled stack of 52 standard cards matching a normal deck.
    /// </summary>
    public class Deck
    {
        private readonly Stack<Card> _cards;

        public int Size => _cards.Count;
        public List<Card> GetCards() => _cards.ToList();

        public Deck()
        {
            // Build up a standard deck
            var build = new Card[52];
            int iCard = 0;
            for (int iSuit = 1; iSuit <= 4; iSuit++)
            {
                for (int iFace = 2; iFace <= 14; iFace++)
                {
                    build[iCard++] = new Card((Face)iFace, (Suit)iSuit);
                }
            }

            // Shuffle the deck
            var shuffleFrom = InfiniteRng.GetPositiveInt32Array(build.Length);
            var shuffleTo = InfiniteRng.GetPositiveInt32Array(build.Length);
            for (int i = 0; i < shuffleFrom.Length; i++)
            {
                var from = shuffleFrom[i] % 52;
                var to = shuffleTo[i] % 52;
                var swap = build[to];
                build[to] = build[from];
                build[from] = swap;
            }

            // Create the stack
            _cards = new Stack<Card>(build);
        }

        public Deck(byte[] binary)
        {
            Guards.ArgumentNotNull(binary, nameof(binary));

            _cards = new Stack<Card>(binary.Select(_ => new Card(_)));
        }

        public Card DrawCard()
        {
            if (_cards.Count <= 0) throw new InvalidOperationException("Cannot draw a card because the deck is empty");

            return _cards.Pop();
        }

        public override string ToString()
        {
            return $"{_cards.Count} cards";
        }

        public byte[] ToBinary()
        {
            return _cards.Select(_ => _.Key).Reverse().ToArray();
        }
    }
}