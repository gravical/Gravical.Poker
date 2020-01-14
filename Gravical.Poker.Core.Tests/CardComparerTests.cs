using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Gravical.Poker.Core.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CardComparerTests : TestBase
    {
        private static readonly IEqualityComparer<Card> Simple = new CardComparer();
        private static readonly Card SimpleCard = new Card(Face.Five, Suit.Club);

        [TestMethod]
        public void Equals_IEqualityComparerDifferntSuit_ShouldReturnFalse()
        {
            Assert.IsFalse(Simple.Equals(SimpleCard, new Card(SimpleCard.Face, Suit.Spade)));
        }

        [TestMethod]
        public void Equals_IEqualityComparerDifferntFace_ShouldReturnFalse()
        {
            Assert.IsFalse(Simple.Equals(SimpleCard, new Card(Face.Ten, SimpleCard.Suit)));
        }

        [TestMethod]
        public void Equals_IEqualityComparerSame_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.Equals(SimpleCard, new Card(SimpleCard.Face, SimpleCard.Suit)));
        }

        [TestMethod]
        public void GetHashCode_IEqualityComparer_ShouldReturnKnownValue()
        {
            Assert.AreEqual(83, Simple.GetHashCode(SimpleCard));
        }
    }
}