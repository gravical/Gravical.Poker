using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;


namespace Gravical.Poker.Core.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TableFinalTests : TestBase
    {
        private TableFinal Simple = new TableFinal(
            new Card(Face.Two, Suit.Heart),
            new Card(Face.Three, Suit.Club),
            new Card(Face.Four, Suit.Diamond),
            new Card(Face.Five, Suit.Spade),
            new Card(Face.Six, Suit.Heart));

        [TestMethod]
        public void Ctor_Normal_ShouldSetCorrectCards()
        {
            Assert.AreEqual(new Card(Face.Two, Suit.Heart), Simple.First);
            Assert.AreEqual(new Card(Face.Three, Suit.Club), Simple.Second);
            Assert.AreEqual(new Card(Face.Four, Suit.Diamond), Simple.Third);
            Assert.AreEqual(new Card(Face.Five, Suit.Spade), Simple.Turn);
            Assert.AreEqual(new Card(Face.Six, Suit.Heart), Simple.River);
        }

        [TestMethod]
        public void ToArray_Normal_ShouldReturnInOrder()
        {
            var actual = Simple.ToArray();
            Assert.AreEqual(5, actual.Length);
            Assert.AreEqual(new Card(Face.Two, Suit.Heart), actual[0]);
            Assert.AreEqual(new Card(Face.Three, Suit.Club), actual[1]);
            Assert.AreEqual(new Card(Face.Four, Suit.Diamond), actual[2]);
            Assert.AreEqual(new Card(Face.Five, Suit.Spade), actual[3]);
            Assert.AreEqual(new Card(Face.Six, Suit.Heart), actual[4]);
        }

        [TestMethod]
        public void GetAllCards_Normal_ShouldReturnSevenCards()
        {
            var arranged = new Pocket(new Deck());
            var actual = Simple.GetAllCards(arranged);
            Assert.AreEqual(7, actual.Length);
            Assert.AreEqual(new Card(Face.Two, Suit.Heart), actual[0]);
            Assert.AreEqual(new Card(Face.Two, Suit.Heart), actual[0]);
            Assert.AreEqual(new Card(Face.Three, Suit.Club), actual[1]);
            Assert.AreEqual(new Card(Face.Four, Suit.Diamond), actual[2]);
            Assert.AreEqual(new Card(Face.Five, Suit.Spade), actual[3]);
            Assert.AreEqual(new Card(Face.Six, Suit.Heart), actual[4]);
            Assert.AreEqual(arranged.CardA, actual[5]);
            Assert.AreEqual(arranged.CardB, actual[6]);
        }

        [TestMethod]
        public void Contains_DoesNot_ShouldReturnFalse()
        {
            Assert.IsFalse(Simple.Contains(new Card(Face.Ace, Suit.Heart)));
        }

        [TestMethod]
        public void Contains_MatchFirst_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.Contains(new Card(Face.Two, Suit.Heart)));
        }

        [TestMethod]
        public void Contains_MatchSecond_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.Contains(new Card(Face.Three, Suit.Club)));
        }

        [TestMethod]
        public void Contains_MatchThird_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.Contains(new Card(Face.Four, Suit.Diamond)));
        }

        [TestMethod]
        public void Contains_MatchTurn_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.Contains(new Card(Face.Five, Suit.Spade)));
        }

        [TestMethod]
        public void Contains_MatchRiver_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.Contains(new Card(Face.Six, Suit.Heart)));
        }

        [TestMethod]
        public void ToString_Normal_ShouldReturnKnownString()
        {
            Assert.AreEqual("2h 3c 4d 5s 6h", Simple.ToString());
        }
    }
}