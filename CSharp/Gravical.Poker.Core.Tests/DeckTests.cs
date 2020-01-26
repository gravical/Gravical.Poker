using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Gravical.Poker.Core.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DeckTests : TestBase
    {
        private Deck Simple;

        [TestInitialize]
        public new void Initialize()
        {
            base.Initialize();
            Simple = new Deck();
        }

        private void AssertFullDeck(List<Card> cards)
        {
            Assert.AreEqual(52, cards.Count);
            for (int iSuit = 1; iSuit <= 4; iSuit++)
            {
                for (int iFace = 2; iFace <= 14; iFace++)
                {
                    Assert.AreEqual(1, cards.Count(_ => _.Equals(new Card((Face)iFace, (Suit)iSuit))));
                }
            }
        }

        #region Ctor

        [TestMethod]
        public void Ctor_Default_ShouldCreateDeckOfCorrect52()
        {
            AssertFullDeck(Simple.GetCards());
        }

        [TestMethod]
        public void Ctor_NullBinary_ShouldThrowArgumentNullException()
        {
            ActExpectingArgumentNullException("binary", () => new Deck(null));
        }

        [TestMethod]
        public void Ctor_EmptyBinary_ShouldCreateEmptyDeck()
        {
            Assert.AreEqual(0, new Deck(new byte[0]).Size);
        }

        [TestMethod]
        public void Ctor_InvalidBinaryCard_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("binary", "Card is not valid", () => new Deck(new byte[] { 0xFF, 0xFF }));
        }

        [TestMethod]
        public void Ctor_PartialBinary_ShouldCreatePartialDeck()
        {
            var actual = new Deck(new byte[]
            {
                new Card(Face.Ace, Suit.Spade).Key,
                new Card(Face.King, Suit.Diamond).Key
            }).GetCards();
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("Kd", actual[0].ToString());
            Assert.AreEqual("As", actual[1].ToString());
        }


        [TestMethod]
        public void Ctor_CircularBinary_ShouldMatch()
        {
            var a = new Deck(new byte[]
            {
                new Card(Face.Ace, Suit.Spade).Key,
                new Card(Face.King, Suit.Diamond).Key
            });
            var b = new Deck(a.ToBinary());
            Assert.IsTrue(a.GetCards().SequenceEqual(b.GetCards()));
        }

        [TestMethod]
        public void Ctor_FullBinary_ShouldCreateFullDeck()
        {
            AssertFullDeck(new Deck(new Deck().ToBinary()).GetCards());
        }

        #endregion

        #region Size

        [TestMethod]
        public void Size_Empty_ShouldReturnZero()
        {
            for (int i = 0; i < 52; i++) Simple.DrawCard();
            Assert.AreEqual(0, Simple.Size);
        }

        [TestMethod]
        public void Size_Half_ShouldReturn26()
        {
            for (int i = 0; i < 26; i++) Simple.DrawCard();
            Assert.AreEqual(26, Simple.Size);
        }

        [TestMethod]
        public void GetCards_Complete_ShouldReturn52()
        {
            Assert.AreEqual(52, Simple.Size);
        }

        #endregion

        #region GetCards

        [TestMethod]
        public void GetCards_Empty_ShouldReturnEmpty()
        {
            for (int i = 0; i < 52; i++) Simple.DrawCard();
            Assert.AreEqual(0, Simple.GetCards().Count);
        }

        [TestMethod]
        public void GetCards_Single_ShouldReturnIt()
        {
            for (int i = 0; i < 51; i++) Simple.DrawCard();
            var expected = Simple.GetCards().Single();
            var actual = Simple.DrawCard();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetCards_Complete_ShouldReturnComplete()
        {
            var cards = Simple.GetCards();
            AssertFullDeck(cards);
        }

        #endregion

        #region DrawCard

        [TestMethod]
        public void DrawCard_EmptyDeck_ShouldThrowInvalidOperationException()
        {
            for (int i = 0; i < 52; i++) Simple.DrawCard();
            ActExpectingInvalidOperationException("Cannot draw a card because the deck is empty", () => Simple.DrawCard());
        }

        [TestMethod]
        public void DrawCard_SingleCardLeft_ShouldReturnThatCard()
        {
            for (int i = 0; i < 51; i++) Simple.DrawCard();
            var expected = Simple.GetCards().Single();
            var actual = Simple.DrawCard();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DrawCard_FullDeck_ShouldReturnValidCard()
        {
            var card = Simple.DrawCard();
            Assert.IsTrue(card.Suit.IsValid());
            Assert.IsTrue(card.Face.IsValid());
        }

        #endregion

        #region ToString

        [TestMethod]
        public void ToString_FullDeck_ShouldReturnCorrectValue()
        {
            Assert.AreEqual("52 cards", new Deck().ToString());
        }

        [TestMethod]
        public void ToString_PartialDeck_ShouldReturnCorrectValue()
        {
            var arrange = new Deck();
            arrange.DrawCard();
            arrange.DrawCard();
            Assert.AreEqual("50 cards", arrange.ToString());
        }

        #endregion

        #region ToBinary

        [TestMethod]
        public void ToBinary_Empty_ShouldReturnEmpty()
        {
            Assert.AreEqual(0, new Deck(new byte[0]).ToBinary().Length);
        }

        [TestMethod]
        public void ToBinary_Full_ShouldReturnFull()
        {
            var actual = new Deck().ToBinary().OrderBy(_ => _).ToArray();
            Assert.AreEqual(52, actual.Length);
            Assert.AreEqual("ISIjJDEyMzRBQkNEUVJTVGFiY2RxcnN0gYKDhJGSk5ShoqOksbKztMHCw8TR0tPU4eLj5A==", Convert.ToBase64String(actual));
        }

        [TestMethod]
        public void ToBinary_Partial_ShouldReturnPartial()
        {
            var actual = new Deck(new byte[] { 0x21, 0x84 }).ToBinary().OrderBy(_ => _).ToArray();
            Assert.AreEqual(2, actual.Length);
            Assert.AreEqual(0x21, actual[0]);
            Assert.AreEqual(0x84, actual[1]);
        }

        #endregion
    }
}
