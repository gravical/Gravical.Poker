namespace Gravical.Poker.Core
{
    /// <summary>
    /// Holds all community cards from a Texas Hold-em table.
    /// </summary>
    public class TableFinal
    {
        public Card First { get; }
        public Card Second { get; }
        public Card Third { get; }
        public Card Turn { get; }
        public Card River { get; }

        public TableFinal(Card first, Card second, Card third, Card turn, Card river)
        {
            First = first;
            Second = second;
            Third = third;
            Turn = turn;
            River = river;
        }

        public Card[] ToArray()
        {
            return new[] { First, Second, Third, Turn, River };
        }

        public Card[] GetAllCards(Pocket pocket)
        {
            return new[]
            {
                First,
                Second,
                Third,
                Turn,
                River,
                pocket.CardA,
                pocket.CardB,
            };
        }

        public bool Contains(Card card)
        {
            return card.Equals(First)
                   || card.Equals(Second)
                   || card.Equals(Third)
                   || card.Equals(Turn)
                   || card.Equals(River);
        }

        public override string ToString() => ToArray().ToCardString();
    }
}