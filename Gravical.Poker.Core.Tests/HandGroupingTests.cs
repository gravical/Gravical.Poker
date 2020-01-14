using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Linq;


namespace Gravical.Poker.Core.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HandGroupingTests : TestBase
    {
        private Card[] SimpleCards = new Card[]
        {
            new Card(Face.Ace, Suit.Heart),
            new Card(Face.King, Suit.Diamond),
            new Card(Face.Queen, Suit.Club),
            new Card(Face.Jack, Suit.Spade),
            new Card(Face.Ten, Suit.Heart),
            new Card(Face.Nine, Suit.Diamond),
            new Card(Face.Eight, Suit.Club),
            new Card(Face.Seven, Suit.Spade),
        };

        [TestMethod]
        public void Ctor_Null_ShouldThrowArgumentNullException()
        {
            ActExpectingArgumentNullException("cards", () => new HandGrouping(null));
        }

        [TestMethod]
        public void Ctor_Empty_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("cards", "Only 5-7 cards can be grouped", () => new HandGrouping(new Card[0]));
        }

        [TestMethod]
        public void Ctor_TooFewCards_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("cards", "Only 5-7 cards can be grouped", () => new HandGrouping(SimpleCards.Take(4).ToArray()));
        }

        [TestMethod]
        public void Ctor_TooManyCards_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("cards", "Only 5-7 cards can be grouped", () => new HandGrouping(SimpleCards));
        }

        [TestMethod]
        public void Ctor_DuplicateCard_ShouldThrowArgumentException()
        {
            var arrange = SimpleCards.Take(5).ToArray();
            arrange[1] = arrange[0];
            ActExpectingArgumentException("cards", "Duplicate cards cannot be grouped", () => new HandGrouping(arrange));
        }

        [TestMethod]
        public void Ctor_InvalidCard_ShouldThrowArgumentException()
        {
            var arrange = SimpleCards.Take(5).ToArray();
            arrange[1] = new Card();
            ActExpectingArgumentException("cards", "Invalid cards cannot be grouped", () => new HandGrouping(arrange));
        }

        [TestMethod]
        public void Ctor_FourOfAKind_ShouldReturnIt()
        {
            var actual = new HandGrouping(new[]
            {
                new Card(Face.Ace, Suit.Heart),
                new Card(Face.Ace, Suit.Diamond),
                new Card(Face.Ace, Suit.Club),
                new Card(Face.Ace, Suit.Spade),
                new Card(Face.King, Suit.Heart),
                new Card(Face.Queen, Suit.Heart),
                new Card(Face.Jack, Suit.Heart),
            });
            Assert.AreEqual(3, actual.Single.Count);
            Assert.AreEqual(0, actual.Pair.Count);
            Assert.AreEqual(0, actual.ThreeOfAKind.Count);
            Assert.AreEqual(1, actual.FourOfAKind.Count);
            Assert.AreEqual(4, actual.FourOfAKind[0].Count);
            Assert.AreEqual("Ah", actual.FourOfAKind[0][0].ToString());
            Assert.AreEqual("Ad", actual.FourOfAKind[0][1].ToString());
            Assert.AreEqual("Ac", actual.FourOfAKind[0][2].ToString());
            Assert.AreEqual("As", actual.FourOfAKind[0][3].ToString());
        }

        [TestMethod]
        public void Ctor_ThreeOfAKind_ShouldReturnIt()
        {
            var actual = new HandGrouping(new[]
            {
                new Card(Face.Ace, Suit.Heart),
                new Card(Face.Ace, Suit.Diamond),
                new Card(Face.Ace, Suit.Club),
                new Card(Face.King, Suit.Heart),
                new Card(Face.Queen, Suit.Heart),
                new Card(Face.Jack, Suit.Heart),
                new Card(Face.Ten, Suit.Heart),
            });
            Assert.AreEqual(4, actual.Single.Count);
            Assert.AreEqual(0, actual.Pair.Count);
            Assert.AreEqual(1, actual.ThreeOfAKind.Count);
            Assert.AreEqual("Ah", actual.ThreeOfAKind[0][0].ToString());
            Assert.AreEqual("Ad", actual.ThreeOfAKind[0][1].ToString());
            Assert.AreEqual("Ac", actual.ThreeOfAKind[0][2].ToString());
            Assert.AreEqual(0, actual.FourOfAKind.Count);
        }

        [TestMethod]
        public void Ctor_MultipleThreeOfAKind_ShouldReturnInCorrectOrder()
        {
            var actual = new HandGrouping(new[]
            {
                new Card(Face.Ace, Suit.Heart),
                new Card(Face.Ace, Suit.Diamond),
                new Card(Face.Ace, Suit.Club),
                new Card(Face.King, Suit.Heart),
                new Card(Face.King, Suit.Diamond),
                new Card(Face.King, Suit.Club),
                new Card(Face.Ten, Suit.Heart),
            });
            Assert.AreEqual(1, actual.Single.Count);
            Assert.AreEqual(0, actual.Pair.Count);
            Assert.AreEqual(2, actual.ThreeOfAKind.Count);
            Assert.AreEqual(3, actual.ThreeOfAKind[0].Count);
            Assert.AreEqual("Ah", actual.ThreeOfAKind[0][0].ToString());
            Assert.AreEqual("Ad", actual.ThreeOfAKind[0][1].ToString());
            Assert.AreEqual("Ac", actual.ThreeOfAKind[0][2].ToString());
            Assert.AreEqual(3, actual.ThreeOfAKind[1].Count);
            Assert.AreEqual("Kh", actual.ThreeOfAKind[1][0].ToString());
            Assert.AreEqual("Kd", actual.ThreeOfAKind[1][1].ToString());
            Assert.AreEqual("Kc", actual.ThreeOfAKind[1][2].ToString());
            Assert.AreEqual(0, actual.FourOfAKind.Count);
        }

        [TestMethod]
        public void Ctor_Pair_ShouldReturnIt()
        {
            var actual = new HandGrouping(new[]
            {
                new Card(Face.Ace, Suit.Heart),
                new Card(Face.Ace, Suit.Diamond),
                new Card(Face.Queen, Suit.Club),
                new Card(Face.Jack, Suit.Spade),
                new Card(Face.Ten, Suit.Heart),
                new Card(Face.Nine, Suit.Diamond),
                new Card(Face.Eight, Suit.Club),
            });
            Assert.AreEqual(5, actual.Single.Count);
            Assert.AreEqual(1, actual.Single[0].Count);
            Assert.AreEqual(1, actual.Single[1].Count);
            Assert.AreEqual(1, actual.Single[2].Count);
            Assert.AreEqual(1, actual.Single[3].Count);
            Assert.AreEqual(1, actual.Single[4].Count);
            Assert.AreEqual("Qc", actual.Single[0][0].ToString());
            Assert.AreEqual("Js", actual.Single[1][0].ToString());
            Assert.AreEqual(1, actual.Pair.Count);
            Assert.AreEqual(2, actual.Pair[0].Count);
            Assert.AreEqual("Ah", actual.Pair[0][0].ToString());
            Assert.AreEqual("Ad", actual.Pair[0][1].ToString());
            Assert.AreEqual(0, actual.ThreeOfAKind.Count);
            Assert.AreEqual(0, actual.FourOfAKind.Count);
        }

        [TestMethod]
        public void Ctor_MultiplePair_ShouldReturnInCorrectOrder()
        {
            var actual = new HandGrouping(new[]
            {
                new Card(Face.Ace, Suit.Heart),
                new Card(Face.Ace, Suit.Diamond),
                new Card(Face.King, Suit.Heart),
                new Card(Face.King, Suit.Diamond),
                new Card(Face.Queen, Suit.Heart),
                new Card(Face.Queen, Suit.Diamond),
                new Card(Face.Jack, Suit.Heart),
            });
            Assert.AreEqual(1, actual.Single.Count);
            Assert.AreEqual(1, actual.Single[0].Count);
            Assert.AreEqual("Jh", actual.Single[0][0].ToString());
            Assert.AreEqual(3, actual.Pair.Count);
            Assert.AreEqual(2, actual.Pair[0].Count);
            Assert.AreEqual("Ah", actual.Pair[0][0].ToString());
            Assert.AreEqual("Ad", actual.Pair[0][1].ToString());
            Assert.AreEqual(2, actual.Pair[1].Count);
            Assert.AreEqual("Kh", actual.Pair[1][0].ToString());
            Assert.AreEqual("Kd", actual.Pair[1][1].ToString());
            Assert.AreEqual(2, actual.Pair[2].Count);
            Assert.AreEqual("Qh", actual.Pair[2][0].ToString());
            Assert.AreEqual("Qd", actual.Pair[2][1].ToString());
            Assert.AreEqual(0, actual.ThreeOfAKind.Count);
            Assert.AreEqual(0, actual.FourOfAKind.Count);
        }

        [TestMethod]
        public void Ctor_Singles_ShouldReturnInCorrectOrder()
        {
            var actual = new HandGrouping(SimpleCards.Take(7).ToArray());
            Assert.AreEqual(7, actual.Single.Count);
            Assert.AreEqual(1, actual.Single[0].Count);
            Assert.AreEqual(1, actual.Single[1].Count);
            Assert.AreEqual(1, actual.Single[2].Count);
            Assert.AreEqual(1, actual.Single[3].Count);
            Assert.AreEqual(1, actual.Single[4].Count);
            Assert.AreEqual(1, actual.Single[5].Count);
            Assert.AreEqual(1, actual.Single[6].Count);
            Assert.AreEqual("Ah", actual.Single[0][0].ToString());
            Assert.AreEqual("Kd", actual.Single[1][0].ToString());
            Assert.AreEqual("Qc", actual.Single[2][0].ToString());
            Assert.AreEqual("Js", actual.Single[3][0].ToString());
            Assert.AreEqual("Th", actual.Single[4][0].ToString());
            Assert.AreEqual("9d", actual.Single[5][0].ToString());
            Assert.AreEqual("8c", actual.Single[6][0].ToString());
            Assert.AreEqual(0, actual.Pair.Count);
            Assert.AreEqual(0, actual.ThreeOfAKind.Count);
            Assert.AreEqual(0, actual.FourOfAKind.Count);
        }

        [TestMethod]
        public void Ctor_FourOfAKindWithHighCards_ShouldReturnCorrectly()
        {
            var actual = new HandGrouping(new[]
            {
                new Card(Face.Ace, Suit.Heart),
                new Card(Face.Ace, Suit.Diamond),
                new Card(Face.Ace, Suit.Club),
                new Card(Face.Ace, Suit.Spade),
                new Card(Face.King, Suit.Heart),
                new Card(Face.Queen, Suit.Heart),
                new Card(Face.Jack, Suit.Heart),
            });
            Assert.AreEqual(3, actual.Single.Count);
            Assert.AreEqual(1, actual.Single[0].Count);
            Assert.AreEqual(1, actual.Single[1].Count);
            Assert.AreEqual(1, actual.Single[2].Count);
            Assert.AreEqual("Kh", actual.Single[0][0].ToString());
            Assert.AreEqual("Qh", actual.Single[1][0].ToString());
            Assert.AreEqual("Jh", actual.Single[2][0].ToString());
            Assert.AreEqual(0, actual.Pair.Count);
            Assert.AreEqual(0, actual.ThreeOfAKind.Count);
            Assert.AreEqual(1, actual.FourOfAKind.Count);
            Assert.AreEqual(4, actual.FourOfAKind[0].Count);
            Assert.AreEqual("Ah", actual.FourOfAKind[0][0].ToString());
            Assert.AreEqual("Ad", actual.FourOfAKind[0][1].ToString());
            Assert.AreEqual("Ac", actual.FourOfAKind[0][2].ToString());
            Assert.AreEqual("As", actual.FourOfAKind[0][3].ToString());
        }

        [TestMethod]
        public void Ctor_FullHouseWithHighCards_ShouldReturnCorrectly()
        {
            var actual = new HandGrouping(new[]
            {
                new Card(Face.Ace, Suit.Heart),
                new Card(Face.Ace, Suit.Diamond),
                new Card(Face.Ace, Suit.Club),
                new Card(Face.King, Suit.Heart),
                new Card(Face.King, Suit.Diamond),
                new Card(Face.Queen, Suit.Heart),
                new Card(Face.Jack, Suit.Heart),
            });
            Assert.AreEqual(2, actual.Single.Count);
            Assert.AreEqual(1, actual.Single[0].Count);
            Assert.AreEqual(1, actual.Single[1].Count);
            Assert.AreEqual("Qh", actual.Single[0][0].ToString());
            Assert.AreEqual("Jh", actual.Single[1][0].ToString());
            Assert.AreEqual(1, actual.Pair.Count);
            Assert.AreEqual(2, actual.Pair[0].Count);
            Assert.AreEqual("Kh", actual.Pair[0][0].ToString());
            Assert.AreEqual("Kd", actual.Pair[0][1].ToString());
            Assert.AreEqual(1, actual.ThreeOfAKind.Count);
            Assert.AreEqual(3, actual.ThreeOfAKind[0].Count);
            Assert.AreEqual("Ah", actual.ThreeOfAKind[0][0].ToString());
            Assert.AreEqual("Ad", actual.ThreeOfAKind[0][1].ToString());
            Assert.AreEqual("Ac", actual.ThreeOfAKind[0][2].ToString());
            Assert.AreEqual(0, actual.FourOfAKind.Count);
        }

        [TestMethod]
        public void Ctor_ThreeOfAKindWithHighCards_ShouldReturnCorrectly()
        {
            var actual = new HandGrouping(new[]
            {
                new Card(Face.Ace, Suit.Heart),
                new Card(Face.Ace, Suit.Diamond),
                new Card(Face.Ace, Suit.Club),
                new Card(Face.King, Suit.Heart),
                new Card(Face.Queen, Suit.Heart),
                new Card(Face.Jack, Suit.Heart),
                new Card(Face.Ten, Suit.Heart),
            });
            Assert.AreEqual(4, actual.Single.Count);
            Assert.AreEqual(1, actual.Single[0].Count);
            Assert.AreEqual(1, actual.Single[1].Count);
            Assert.AreEqual(1, actual.Single[2].Count);
            Assert.AreEqual(1, actual.Single[3].Count);
            Assert.AreEqual("Kh", actual.Single[0][0].ToString());
            Assert.AreEqual("Qh", actual.Single[1][0].ToString());
            Assert.AreEqual("Jh", actual.Single[2][0].ToString());
            Assert.AreEqual("Th", actual.Single[3][0].ToString());
            Assert.AreEqual(0, actual.Pair.Count);
            Assert.AreEqual(1, actual.ThreeOfAKind.Count);
            Assert.AreEqual(3, actual.ThreeOfAKind[0].Count);
            Assert.AreEqual("Ah", actual.ThreeOfAKind[0][0].ToString());
            Assert.AreEqual("Ad", actual.ThreeOfAKind[0][1].ToString());
            Assert.AreEqual("Ac", actual.ThreeOfAKind[0][2].ToString());
            Assert.AreEqual(0, actual.FourOfAKind.Count);
        }

        [TestMethod]
        public void Ctor_TwoPairWithHighCards_ShouldReturnCorrectly()
        {
            var actual = new HandGrouping(new[]
            {
                new Card(Face.Ace, Suit.Heart),
                new Card(Face.Ace, Suit.Diamond),
                new Card(Face.King, Suit.Heart),
                new Card(Face.King, Suit.Diamond),
                new Card(Face.Queen, Suit.Heart),
                new Card(Face.Jack, Suit.Heart),
                new Card(Face.Ten, Suit.Heart),
            });
            Assert.AreEqual(3, actual.Single.Count);
            Assert.AreEqual(1, actual.Single[0].Count);
            Assert.AreEqual(1, actual.Single[1].Count);
            Assert.AreEqual(1, actual.Single[2].Count);
            Assert.AreEqual("Qh", actual.Single[0][0].ToString());
            Assert.AreEqual("Jh", actual.Single[1][0].ToString());
            Assert.AreEqual("Th", actual.Single[2][0].ToString());
            Assert.AreEqual(2, actual.Pair.Count);
            Assert.AreEqual(2, actual.Pair[0].Count);
            Assert.AreEqual("Ah", actual.Pair[0][0].ToString());
            Assert.AreEqual("Ad", actual.Pair[0][1].ToString());
            Assert.AreEqual(2, actual.Pair[1].Count);
            Assert.AreEqual("Kh", actual.Pair[1][0].ToString());
            Assert.AreEqual("Kd", actual.Pair[1][1].ToString());
            Assert.AreEqual(0, actual.ThreeOfAKind.Count);
            Assert.AreEqual(0, actual.FourOfAKind.Count);
        }
    }
}
