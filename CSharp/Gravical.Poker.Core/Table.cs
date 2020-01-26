using System;
using System.Linq;

namespace Gravical.Poker.Core
{
    /// <summary>
    /// Table represents the state of a Texas Holdem game from preflop through the river
    /// </summary>
    public class Table : ITable
    {
        private int _preFlopDeckSize;

        public Deck Deck { get; }
        public TableStatus Status { get; private set; }
        public Card? First { get; private set; }
        public Card? Second { get; private set; }
        public Card? Third { get; private set; }
        public Card? Turn { get; private set; }
        public Card? River { get; private set; }

        public Guid Id { get; } = Guid.NewGuid();

        public Table()
        {
            Deck = new Deck();
            Status = TableStatus.BeforeFlop;
        }

        public Table(Deck deck, TableStatus status, byte[] community)
        {
            Guards.ArgumentNotNull(deck, nameof(deck));
            Guards.ArgumentSuccess(status.IsValid(), nameof(status));
            Guards.ArgumentHasExactSize(community, 5, nameof(community));

            this.Deck = deck;
            this.Status = status;
            this.First = community[0] == 0 ? (Card?)null : new Card(community[0]);
            this.Second = community[1] == 0 ? (Card?)null : new Card(community[1]);
            this.Third = community[2] == 0 ? (Card?)null : new Card(community[2]);
            this.Turn = community[3] == 0 ? (Card?)null : new Card(community[3]);
            this.River = community[4] == 0 ? (Card?)null : new Card(community[4]);

            var duplicate = community.Where(_ => _ != 0).GroupBy(_ => _).Any(_ => _.Count() > 1);
            Guards.ArgumentSuccess(!duplicate, nameof(community), "Duplicate card found");

            var mismatch = $"Community cards don't match table status: {status}, ";
            switch (status)
            {
                case TableStatus.BeforeFlop:
                    Guards.ArgumentSuccess(First == null, nameof(community), mismatch + "First");
                    Guards.ArgumentSuccess(Second == null, nameof(community), mismatch + "Second");
                    Guards.ArgumentSuccess(Third == null, nameof(community), mismatch + "Third");
                    Guards.ArgumentSuccess(Turn == null, nameof(community), mismatch + "Turn");
                    Guards.ArgumentSuccess(River == null, nameof(community), mismatch + "River");
                    break;
                case TableStatus.BeforeTurn:
                    Guards.ArgumentSuccess(First != null, nameof(community), mismatch + "First");
                    Guards.ArgumentSuccess(Second != null, nameof(community), mismatch + "Second");
                    Guards.ArgumentSuccess(Third != null, nameof(community), mismatch + "Third");
                    Guards.ArgumentSuccess(Turn == null, nameof(community), mismatch + "Turn");
                    Guards.ArgumentSuccess(River == null, nameof(community), mismatch + "River");
                    break;
                case TableStatus.BeforeRiver:
                    Guards.ArgumentSuccess(First != null, nameof(community), mismatch + "First");
                    Guards.ArgumentSuccess(Second != null, nameof(community), mismatch + "Second");
                    Guards.ArgumentSuccess(Third != null, nameof(community), mismatch + "Third");
                    Guards.ArgumentSuccess(Turn != null, nameof(community), mismatch + "Turn");
                    Guards.ArgumentSuccess(River == null, nameof(community), mismatch + "River");
                    break;
                case TableStatus.Complete:
                    Guards.ArgumentSuccess(First != null, nameof(community), mismatch + "First");
                    Guards.ArgumentSuccess(Second != null, nameof(community), mismatch + "Second");
                    Guards.ArgumentSuccess(Third != null, nameof(community), mismatch + "Third");
                    Guards.ArgumentSuccess(Turn != null, nameof(community), mismatch + "Turn");
                    Guards.ArgumentSuccess(River != null, nameof(community), mismatch + "River");
                    break;
            }
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void DealTheFlop()
        {
            AssertStatus(TableStatus.BeforeFlop);
            Guards.Assert(Deck.Size >= 9, "There are not enough cards left in the deck to deal a table");
            _preFlopDeckSize = Deck.Size;
            Deck.DrawCard();
            First = Deck.DrawCard();
            Second = Deck.DrawCard();
            Third = Deck.DrawCard();
            Status = TableStatus.BeforeTurn;
        }

        public void DealTheTurn()
        {
            AssertStatus(TableStatus.BeforeTurn);
            AssertDeckSize(4);
            Deck.DrawCard();
            Turn = Deck.DrawCard();
            Status = TableStatus.BeforeRiver;
        }

        public void DealTheRiver()
        {
            AssertStatus(TableStatus.BeforeRiver);
            AssertDeckSize(6);
            Deck.DrawCard();
            River = Deck.DrawCard();
            Status = TableStatus.Complete;
        }

        private void AssertStatus(TableStatus status)
        {
            Guards.Assert(Status == status, $"Unexpected dealing order. Expected {status} but actual is {Status}");
        }

        private void AssertDeckSize(int plus)
        {
            Guards.Assert(Deck.Size + plus == _preFlopDeckSize, "Deck has unexpectedly changed");
        }

        public TableFinal ToFinal()
        {
            Guards.Assert(Status == TableStatus.Complete, "Table is not complete");
            return new TableFinal(First.Value, Second.Value, Third.Value, Turn.Value, River.Value);
        }

        public override string ToString()
        {
            switch (Status)
            {
                case TableStatus.BeforeFlop:
                    return "Table is empty";
                case TableStatus.BeforeTurn:
                    return ToString(First, Second, Third);
                case TableStatus.BeforeRiver:
                    return ToString(First, Second, Third, Turn);
                case TableStatus.Complete:
                    return ToString(First, Second, Third, Turn, River);
                default:
                    return "Table is invalid";
            }
        }

        private string ToString(params Card?[] cards) => string.Join(" ", cards);

        public byte[] GetBinaryCommunity()
        {
            return new byte[]
            {
                First?.Key ?? 0,
                Second?.Key ?? 0,
                Third?.Key ?? 0,
                Turn?.Key ?? 0,
                River?.Key ?? 0,
            };
        }
    }
}