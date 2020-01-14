using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;


namespace Gravical.Poker.Core.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TableTests : TestBase
    {
        private Table Simple;

        [TestInitialize]
        public new void Initialize()
        {
            base.Initialize();
            Simple = new Table();
        }

        #region Ctor

        [TestMethod]
        public void Ctor_Default_ShouldConstruct()
        {
            new Table();
        }

        [TestMethod]
        public void Ctor_Default_ShouldCreateFullDeck()
        {
            var arrange = new Table();
            Assert.AreEqual(52, arrange.Deck.Size);
        }

        [TestMethod]
        public void Ctor_NullDeck_ShouldThrowArgumentNullException()
        {
            ActExpectingArgumentNullException("deck", () => new Table(null, TableStatus.BeforeFlop, new byte[5]));
        }

        [TestMethod]
        public void Ctor_InvalidStatus_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("status", () => new Table(Simple.Deck, (TableStatus)0, new byte[5]));
        }

        [TestMethod]
        public void Ctor_NullCommunity_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("community", "Value must be exactly 5 bytes", () => new Table(Simple.Deck, TableStatus.BeforeFlop, null));
        }

        [TestMethod]
        public void Ctor_EmptyCommunity_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("community", "Value must be exactly 5 bytes", () => new Table(Simple.Deck, TableStatus.BeforeFlop, new byte[0]));
        }

        [TestMethod]
        public void Ctor_CommunityToShort_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("community", "Value must be exactly 5 bytes", () => new Table(Simple.Deck, TableStatus.BeforeFlop, new byte[4]));
        }

        [TestMethod]
        public void Ctor_CommunityToLong_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("community", "Value must be exactly 5 bytes", () => new Table(Simple.Deck, TableStatus.BeforeFlop, new byte[6]));
        }

        [TestMethod]
        public void Ctor_CommunityInvalidCard_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("binary", "Card is not valid", () => new Table(Simple.Deck, TableStatus.BeforeFlop, new byte[] { 0xFF, 0xFF, 0xFF, 0x00, 0x00 }));
        }

        [TestMethod]
        public void Ctor_CommunityBeforeFlop_ShouldSetNone()
        {
            var arrange = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 };
            var actual = new Table(Simple.Deck, TableStatus.BeforeFlop, arrange);
            Assert.IsNull(actual.First);
            Assert.IsNull(actual.Second);
            Assert.IsNull(actual.Third);
            Assert.IsNull(actual.Turn);
            Assert.IsNull(actual.River);
        }

        [TestMethod]
        public void Ctor_CommunityBeforeTurn_ShouldSetOnlyFlop()
        {
            var arrange = new byte[] { 0x21, 0x31, 0x41, 0x00, 0x00 };
            var actual = new Table(Simple.Deck, TableStatus.BeforeTurn, arrange);
            Assert.AreEqual("2h", actual.First.Value.ToString());
            Assert.AreEqual("3h", actual.Second.Value.ToString());
            Assert.AreEqual("4h", actual.Third.Value.ToString());
            Assert.IsNull(actual.Turn);
            Assert.IsNull(actual.River);
        }

        [TestMethod]
        public void Ctor_CommunityBeforeRiver_ShouldSetOnlyFlopAndTurn()
        {
            var arrange = new byte[] { 0x21, 0x31, 0x41, 0x51, 0x00 };
            var actual = new Table(Simple.Deck, TableStatus.BeforeRiver, arrange);
            Assert.AreEqual("2h", actual.First.Value.ToString());
            Assert.AreEqual("3h", actual.Second.Value.ToString());
            Assert.AreEqual("4h", actual.Third.Value.ToString());
            Assert.AreEqual("5h", actual.Turn.Value.ToString());
            Assert.IsNull(actual.River);
        }

        [TestMethod]
        public void Ctor_CommunityComplete_ShouldSetAll()
        {
            var arrange = new byte[] { 0x21, 0x31, 0x41, 0x51, 0x61 };
            var actual = new Table(Simple.Deck, TableStatus.Complete, arrange);
            Assert.AreEqual("2h", actual.First.Value.ToString());
            Assert.AreEqual("3h", actual.Second.Value.ToString());
            Assert.AreEqual("4h", actual.Third.Value.ToString());
            Assert.AreEqual("5h", actual.Turn.Value.ToString());
            Assert.AreEqual("6h", actual.River.Value.ToString());
        }

        [TestMethod]
        public void Ctor_Serialized_ShouldSetDeck()
        {
            var actual = new Table(Simple.Deck, TableStatus.BeforeFlop, new byte[5]);
            Assert.IsTrue(Simple.Deck.GetCards().SequenceEqual(actual.Deck.GetCards()));
        }

        [TestMethod]
        public void Ctor_Serialized_ShouldSetStatus()
        {
            var arrange = new byte[] { 0x21, 0x31, 0x41, 0x51, 0x61 };
            Assert.AreEqual(TableStatus.Complete, new Table(Simple.Deck, TableStatus.Complete, arrange).Status);
        }

        [TestMethod]
        public void Ctor_DuplicateCommunityCard_ShouldThrowArgumentException()
        {
            var arrange = new byte[] { 0x21, 0x51, 0x41, 0x51, 0x61 };
            ActExpectingArgumentException("community", "Duplicate card found", () => new Table(Simple.Deck, TableStatus.Complete, arrange));
        }

        private void AssesrtCtorMismatch(TableStatus status, string position, byte[] community)
        {
            ActExpectingArgumentException("community", $"Community cards don't match table status: {status}, {position}", () => new Table(Simple.Deck, status, community));
        }

        [TestMethod]
        public void Ctor_BeforeFlopButFirstNotNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.BeforeFlop, "First", new byte[] { 0x21, 0x00, 0x00, 0x00, 0x00 });
        }

        [TestMethod]
        public void Ctor_BeforeFlopButSecondNotNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.BeforeFlop, "Second", new byte[] { 0x00, 0x21, 0x00, 0x00, 0x00 });
        }

        [TestMethod]
        public void Ctor_BeforeFlopButThirdNotNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.BeforeFlop, "Third", new byte[] { 0x00, 0x00, 0x21, 0x00, 0x00 });
        }

        [TestMethod]
        public void Ctor_BeforeFlopButTurnNotNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.BeforeFlop, "Turn", new byte[] { 0x00, 0x00, 0x00, 0x21, 0x00 });
        }

        [TestMethod]
        public void Ctor_BeforeFlopButRiverNotNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.BeforeFlop, "River", new byte[] { 0x00, 0x00, 0x00, 0x00, 0x21 });
        }

        [TestMethod]
        public void Ctor_BeforeTurnButFirstNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.BeforeTurn, "First", new byte[] { 0x00, 0x31, 0x41, 0x00, 0x00 });
        }

        [TestMethod]
        public void Ctor_BeforeTurnButSecondNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.BeforeTurn, "Second", new byte[] { 0x21, 0x00, 0x41, 0x00, 0x00 });
        }

        [TestMethod]
        public void Ctor_BeforeTurnButThirdNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.BeforeTurn, "Third", new byte[] { 0x21, 0x31, 0x00, 0x00, 0x00 });
        }

        [TestMethod]
        public void Ctor_BeforeTurnButTurnNotNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.BeforeTurn, "Turn", new byte[] { 0x21, 0x31, 0x41, 0x51, 0x00 });
        }

        [TestMethod]
        public void Ctor_BeforeTurnButRiverNotNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.BeforeTurn, "River", new byte[] { 0x21, 0x31, 0x41, 0x00, 0x61 });
        }

        [TestMethod]
        public void Ctor_BeforeRiverButFirstNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.BeforeRiver, "First", new byte[] { 0x00, 0x31, 0x41, 0x51, 0x00 });
        }

        [TestMethod]
        public void Ctor_BeforeRiverButSecondNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.BeforeRiver, "Second", new byte[] { 0x21, 0x00, 0x41, 0x51, 0x00 });
        }

        [TestMethod]
        public void Ctor_BeforeRiverButThirdNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.BeforeRiver, "Third", new byte[] { 0x21, 0x31, 0x00, 0x51, 0x00 });
        }

        [TestMethod]
        public void Ctor_BeforeRiverButTurnNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.BeforeRiver, "Turn", new byte[] { 0x21, 0x31, 0x41, 0x00, 0x00 });
        }

        [TestMethod]
        public void Ctor_BeforeRiverButRiverNotNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.BeforeRiver, "River", new byte[] { 0x21, 0x31, 0x41, 0x51, 0x61 });
        }

        [TestMethod]
        public void Ctor_CompleteButFirstNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.Complete, "First", new byte[] { 0x00, 0x31, 0x41, 0x51, 0x61 });
        }

        [TestMethod]
        public void Ctor_CompleteButSecondNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.Complete, "Second", new byte[] { 0x21, 0x00, 0x41, 0x51, 0x61 });
        }

        [TestMethod]
        public void Ctor_CompleteButThirdNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.Complete, "Third", new byte[] { 0x21, 0x31, 0x00, 0x51, 0x61 });
        }

        [TestMethod]
        public void Ctor_CompleteButTurnNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.Complete, "Turn", new byte[] { 0x21, 0x31, 0x41, 0x00, 0x61 });
        }

        [TestMethod]
        public void Ctor_CompleteButRiverNull_ShouldThrowArgumentException()
        {
            AssesrtCtorMismatch(TableStatus.Complete, "River", new byte[] { 0x21, 0x31, 0x41, 0x51, 0x00 });
        }

        #endregion

        #region Deck

        [TestMethod]
        public void Deck_Set_ShouldNotExist()
        {
            Assert.IsFalse(PropertySetAvailable(typeof(Table), "Deck"));
        }

        [TestMethod]
        public void Deck_Get_ShouldReturnDeck()
        {
            Assert.IsNotNull(Simple.Deck);
        }

        #endregion

        #region Status

        [TestMethod]
        public void Status_Set_ShouldNotExist()
        {
            Assert.IsFalse(PropertySetAvailable(typeof(Table), "Status"));
        }

        [TestMethod]
        public void Status_Initial_ShouldReturnBeforeFlop()
        {
            Assert.AreEqual(TableStatus.BeforeFlop, Simple.Status);
        }

        [TestMethod]
        public void Status_AfterFlop_ShouldReturnBeforeTurn()
        {
            Simple.DealTheFlop();
            Assert.AreEqual(TableStatus.BeforeTurn, Simple.Status);
        }

        [TestMethod]
        public void Status_AfterTurn_ShouldReturnBeforeRiver()
        {
            Simple.DealTheFlop();
            Simple.DealTheTurn();
            Assert.AreEqual(TableStatus.BeforeRiver, Simple.Status);
        }

        [TestMethod]
        public void Status_AfterRiver_ShouldReturnComplete()
        {
            Simple.DealTheFlop();
            Simple.DealTheTurn();
            Simple.DealTheRiver();
            Assert.AreEqual(TableStatus.Complete, Simple.Status);
        }

        #endregion

        #region Cards

        [TestMethod]
        public void First_Set_ShouldNotExist()
        {
            Assert.IsFalse(PropertySetAvailable(typeof(Table), "First"));
        }

        [TestMethod]
        public void Second_Set_ShouldNotExist()
        {
            Assert.IsFalse(PropertySetAvailable(typeof(Table), "Second"));
        }

        [TestMethod]
        public void Third_Set_ShouldNotExist()
        {
            Assert.IsFalse(PropertySetAvailable(typeof(Table), "Third"));
        }

        [TestMethod]
        public void Turn_Set_ShouldNotExist()
        {
            Assert.IsFalse(PropertySetAvailable(typeof(Table), "Turn"));
        }

        [TestMethod]
        public void River_Set_ShouldNotExist()
        {
            Assert.IsFalse(PropertySetAvailable(typeof(Table), "River"));
        }

        [TestMethod]
        public void First_Initial_ShouldReturnNull()
        {
            Assert.IsNull(Simple.First);
        }

        [TestMethod]
        public void Second_Initial_ShouldReturnNull()
        {
            Assert.IsNull(Simple.Second);
        }

        [TestMethod]
        public void Third_Initial_ShouldReturnNull()
        {
            Assert.IsNull(Simple.Third);
        }

        [TestMethod]
        public void Turn_Initial_ShouldReturnNull()
        {
            Assert.IsNull(Simple.Turn);
        }

        [TestMethod]
        public void River_Initial_ShouldReturnNull()
        {
            Assert.IsNull(Simple.River);
        }

        private Table ActDealTheFlop()
        {
            Simple.DealTheFlop();
            return Simple;
        }

        [TestMethod]
        public void First_AfterFlop_ShouldReturnCard()
        {
            Assert.IsNotNull(ActDealTheFlop().First);
        }

        [TestMethod]
        public void Second_AfterFlop_ShouldReturnCard()
        {
            Assert.IsNotNull(ActDealTheFlop().Second);
        }

        [TestMethod]
        public void Third_AfterFlop_ShouldReturnCard()
        {
            Assert.IsNotNull(ActDealTheFlop().Third);
        }

        [TestMethod]
        public void Turn_AfterFlop_ShouldReturnNull()
        {
            Assert.IsNull(ActDealTheFlop().Turn);
        }

        [TestMethod]
        public void River_AfterFlop_ShouldReturnNull()
        {
            Assert.IsNull(ActDealTheFlop().River);
        }

        private Table ActDealTheFlopAndTurn()
        {
            Simple.DealTheFlop();
            Simple.DealTheTurn();
            return Simple;
        }

        [TestMethod]
        public void First_AfterTurn_ShouldReturnCard()
        {
            Assert.IsNotNull(ActDealTheFlopAndTurn().First);
        }

        [TestMethod]
        public void Second_AfterTurn_ShouldReturnCard()
        {
            Assert.IsNotNull(ActDealTheFlopAndTurn().Second);
        }

        [TestMethod]
        public void Third_AfterTurn_ShouldReturnCard()
        {
            Assert.IsNotNull(ActDealTheFlopAndTurn().Third);
        }

        [TestMethod]
        public void Turn_AfterTurn_ShouldReturnCard()
        {
            Assert.IsNotNull(ActDealTheFlopAndTurn().Turn);
        }

        [TestMethod]
        public void River_AfterTurn_ShouldReturnNull()
        {
            Assert.IsNull(ActDealTheFlopAndTurn().River);
        }

        private Table ActDealTheFlopTurnAndRiver()
        {
            Simple.DealTheFlop();
            Simple.DealTheTurn();
            Simple.DealTheRiver();
            return Simple;
        }

        [TestMethod]
        public void First_AfterRiver_ShouldReturnCard()
        {
            Assert.IsNotNull(ActDealTheFlopTurnAndRiver().First);
        }

        [TestMethod]
        public void Second_AfterRiver_ShouldReturnCard()
        {
            Assert.IsNotNull(ActDealTheFlopTurnAndRiver().Second);
        }

        [TestMethod]
        public void Third_AfterRiver_ShouldReturnCard()
        {
            Assert.IsNotNull(ActDealTheFlopTurnAndRiver().Third);
        }

        [TestMethod]
        public void Fourth_AfterRiver_ShouldReturnCard()
        {
            Assert.IsNotNull(ActDealTheFlopTurnAndRiver().Turn);
        }

        [TestMethod]
        public void Fifth_AfterRiver_ShouldReturnCard()
        {
            Assert.IsNotNull(ActDealTheFlopTurnAndRiver().River);
        }

        #endregion

        #region DealTheFlop

        private void ArrangeRemoveCards(int count)
        {
            for (int i = 0; i < count; i++) Simple.Deck.DrawCard();
        }

        [TestMethod]
        public void DealTheFlop_NotEnoughCardsInTheDeck_ShouldThrowInvalidOperationException()
        {
            ArrangeRemoveCards(44);
            ActExpectingInvalidOperationException("There are not enough cards left in the deck to deal a table", () => Simple.DealTheFlop());
        }

        [TestMethod]
        public void DealTheFlop_StatusBeforeTheTurn_ShouldThrowInvalidOperationException()
        {
            ActExpectingInvalidOperationException("Unexpected dealing order. Expected BeforeFlop but actual is BeforeTurn",
                () => ActDealTheFlop().DealTheFlop());
        }

        [TestMethod]
        public void DealTheFlop_StatusBeforeTheRiver_ShouldThrowInvalidOperationException()
        {
            ActExpectingInvalidOperationException("Unexpected dealing order. Expected BeforeFlop but actual is BeforeRiver",
                () => ActDealTheFlopAndTurn().DealTheFlop());
        }

        [TestMethod]
        public void DealTheFlop_StatusComplete_ShouldThrowInvalidOperationException()
        {
            ActExpectingInvalidOperationException("Unexpected dealing order. Expected BeforeFlop but actual is Complete",
                () => ActDealTheFlopTurnAndRiver().DealTheFlop());
        }

        [TestMethod]
        public void DealTheFlop_Normal_ShouldBurnCard()
        {
            Assert.AreEqual(48, ActDealTheFlop().Deck.Size);
        }

        [TestMethod]
        public void DealTheFlop_Normal_ShouldDealCards()
        {
            ActDealTheFlopAndTurn();
            Assert.IsNotNull(Simple.First);
            Assert.IsNotNull(Simple.Second);
            Assert.IsNotNull(Simple.Third);
        }

        #endregion

        #region DealTheTurn

        [TestMethod]
        public void DealTheTurn_DeckChanged_ShouldThrowInvalidOperationException()
        {
            ActDealTheFlop();
            Simple.Deck.DrawCard();
            ActExpectingInvalidOperationException("Deck has unexpectedly changed", () => Simple.DealTheTurn());
        }

        [TestMethod]
        public void DealTheTurn_StatusBeforeTheFlop_ShouldThrowInvalidOperationException()
        {
            ActExpectingInvalidOperationException("Unexpected dealing order. Expected BeforeTurn but actual is BeforeFlop",
                () => Simple.DealTheTurn());
        }

        [TestMethod]
        public void DealTheTurn_StatusBeforeTheRiver_ShouldThrowInvalidOperationException()
        {
            ActExpectingInvalidOperationException("Unexpected dealing order. Expected BeforeTurn but actual is BeforeRiver",
                () => ActDealTheFlopAndTurn().DealTheTurn());
        }

        [TestMethod]
        public void DealTheTurn_StatusComplete_ShouldThrowInvalidOperationException()
        {
            ActExpectingInvalidOperationException("Unexpected dealing order. Expected BeforeTurn but actual is Complete",
                () => ActDealTheFlopTurnAndRiver().DealTheTurn());
        }

        [TestMethod]
        public void DealTheTurn_Normal_ShouldBurnCard()
        {
            Assert.AreEqual(46, ActDealTheFlopAndTurn().Deck.Size);
        }

        [TestMethod]
        public void DealTheTurn_Normal_ShouldDealCard()
        {
            Assert.IsNotNull(ActDealTheFlopAndTurn().Turn);
        }

        #endregion

        #region DealTheRiver

        [TestMethod]
        public void DealTheRiver_DeckChanged_ShouldThrowInvalidOperationException()
        {
            ActDealTheFlopAndTurn();
            Simple.Deck.DrawCard();
            ActExpectingInvalidOperationException("Deck has unexpectedly changed", () => Simple.DealTheRiver());
        }

        [TestMethod]
        public void DealTheRiver_StatusBeforeTheFlop_ShouldThrowInvalidOperationException()
        {
            ActExpectingInvalidOperationException("Unexpected dealing order. Expected BeforeRiver but actual is BeforeFlop",
                () => Simple.DealTheRiver());
        }

        [TestMethod]
        public void DealTheRiver_StatusBeforeTheTurn_ShouldThrowInvalidOperationException()
        {
            ActExpectingInvalidOperationException("Unexpected dealing order. Expected BeforeRiver but actual is BeforeTurn",
                () => ActDealTheFlop().DealTheRiver());
        }

        [TestMethod]
        public void DealTheRiver_StatusComplete_ShouldThrowInvalidOperationException()
        {
            ActExpectingInvalidOperationException("Unexpected dealing order. Expected BeforeRiver but actual is Complete",
                () => ActDealTheFlopTurnAndRiver().DealTheRiver());
        }

        [TestMethod]
        public void DealTheRiver_Normal_ShouldBurnCard()
        {
            Assert.AreEqual(44, ActDealTheFlopTurnAndRiver().Deck.Size);
        }

        [TestMethod]
        public void DealTheRiver_Normal_ShouldDealCard()
        {
            Assert.IsNotNull(ActDealTheFlopTurnAndRiver().River);
        }

        #endregion

        #region ToFinal

        [TestMethod]
        public void ToFinal_BeforeFlop_ShouldThrowInvalidOperationException()
        {
            ActExpectingInvalidOperationException("Table is not complete", () => Simple.ToFinal());
        }

        [TestMethod]
        public void ToFinal_BeforeTurn_ShouldThrowInvalidOperationException()
        {
            ActExpectingInvalidOperationException("Table is not complete", () => ActDealTheFlop().ToFinal());
        }

        [TestMethod]
        public void ToFinal_BeforeRiver_ShouldThrowInvalidOperationException()
        {
            ActExpectingInvalidOperationException("Table is not complete", () => ActDealTheFlopAndTurn().ToFinal());
        }

        [TestMethod]
        public void ToFinal_Complete_ShouldReturnTable()
        {
            var table = ActDealTheFlopTurnAndRiver().ToFinal();
            Assert.AreEqual(Simple.First.Value, table.First);
            Assert.AreEqual(Simple.Second.Value, table.Second);
            Assert.AreEqual(Simple.Third.Value, table.Third);
            Assert.AreEqual(Simple.Turn.Value, table.Turn);
            Assert.AreEqual(Simple.River.Value, table.River);
        }

        #endregion

        #region ToString

        private static readonly Regex LongCardsStringPattern = new Regex(@"^([A-Za-z]+ of [A-Za-z]+(\,\s)?)+$", RegexOptions.Compiled);
        private static readonly Regex ShortCardsStringPattern = new Regex(@"^([23456789TJQKA][hcds]\s?)+$", RegexOptions.Compiled);

        [TestMethod]
        public void ToString_InvalidStatus_ShouldReturnCorrectString()
        {
            SetAnyProperty(Simple, "Status", (TableStatus)0);
            Assert.AreEqual("Table is invalid", Simple.ToString());
        }

        [TestMethod]
        public void ToString_BeforeFlop_ShouldReturnCorrectString()
        {
            Assert.AreEqual("Table is empty", Simple.ToString());
        }

        [TestMethod]
        public void ToString_BeforeTurn_ShouldReturnCorrectString()
        {
            var arrange = ActDealTheFlop().ToString();
            Console.WriteLine(arrange);
            Assert.IsTrue(ShortCardsStringPattern.Match(arrange).Success);
            Assert.AreEqual(2, arrange.Count(_ => _ == ' '));
        }

        [TestMethod]
        public void ToString_BeforeRiver_ShouldReturnCorrectString()
        {
            var arrange = ActDealTheFlopAndTurn().ToString();
            Console.WriteLine(arrange);
            Assert.IsTrue(ShortCardsStringPattern.Match(arrange).Success);
            Assert.AreEqual(3, arrange.Count(_ => _ == ' '));
        }

        [TestMethod]
        public void ToString_BeforeComplete_ShouldReturnCorrectString()
        {
            var arrange = ActDealTheFlopTurnAndRiver().ToString();
            Console.WriteLine(arrange);
            Assert.IsTrue(ShortCardsStringPattern.Match(arrange).Success);
            Assert.AreEqual(4, arrange.Count(_ => _ == ' '));
        }

        #endregion

        #region GetBinaryCommunity

        private void AssertGetBinaryCommunity(bool[] match)
        {
            var actual = Simple.GetBinaryCommunity();
            Assert.AreEqual(5, actual.Length);
            for (int i = 0; i < 5; i++)
            {
                if (match[i])
                    Assert.IsTrue(actual[i] != 0);
                else
                    Assert.IsTrue(actual[i] == 0);
            }
        }

        [TestMethod]
        public void GetBinaryCommunity_BeforeFlop_ShouldReturnAllZero()
        {
            AssertGetBinaryCommunity(new[] { false, false, false, false, false });
        }

        [TestMethod]
        public void GetBinaryCommunity_AfterFlop_ShouldReturnFlopAndZeros()
        {
            Simple.DealTheFlop();
            AssertGetBinaryCommunity(new[] { true, true, true, false, false });
        }

        [TestMethod]
        public void GetBinaryCommunity_AfterTurn_ShouldReturnFlopTurnAndZero()
        {
            Simple.DealTheFlop();
            Simple.DealTheTurn();
            AssertGetBinaryCommunity(new[] { true, true, true, true, false });
        }

        [TestMethod]
        public void GetBinaryCommunity_Complete_ShouldReturnAll()
        {
            Simple.DealTheFlop();
            Simple.DealTheTurn();
            Simple.DealTheRiver();
            AssertGetBinaryCommunity(new[] { true, true, true, true, true });
        }

        #endregion
    }
}