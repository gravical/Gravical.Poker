namespace Gravical.Poker.Core
{
    /// <summary>
    /// Pocket holds two distinct poker cards for a player.
    /// </summary>
    public class Pocket
    {
        public Card CardA { get; }
        public Card CardB { get; }

        public Pocket(Deck deck)
        {
            Guards.ArgumentNotNull(deck, nameof(deck));

            CardA = deck.DrawCard();
            CardB = deck.DrawCard();
        }

        public Pocket(byte[] binary)
        {
            Guards.ArgumentHasExactSize(binary, 2, nameof(binary));
            CardA = new Card(binary[0]);
            CardB = new Card(binary[1]);
        }

        public override string ToString()
        {
            return $"{CardA} {CardB}";
        }

        public Card[] ToArray()
        {
            return new[] { CardA, CardB };
        }

        public bool IsTrumped(Pocket by)
        {
            Guards.ArgumentNotNull(by, nameof(by));

            return CardA.Equals(by.CardA) || CardA.Equals(by.CardB) || CardB.Equals(by.CardA) || CardB.Equals(by.CardB);
        }

        public bool IsTrumped(TableFinal by)
        {
            Guards.ArgumentNotNull(by, nameof(by));

            return by.Contains(CardA) || by.Contains(CardB);
        }

        public byte[] ToBinary()
        {
            return new byte[] { CardA.Key, CardB.Key };
        }
    }
}