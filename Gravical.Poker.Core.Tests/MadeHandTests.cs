using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace Gravical.Poker.Core.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MadeHandTests : TestBase
    {
        private readonly Card[] HighHand = "2h 3d 4c 5s 6h".ParseCards();

        [TestMethod]
        public void Ctor_InvalidType_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("type", () => new MadeHand((HandTypes)(-1), HighHand));
        }

        [TestMethod]
        public void Ctor_NullPlayed_ShouldThrowArgumentNullException()
        {
            ActExpectingArgumentException("played", () => new MadeHand(HandTypes.HighCard, null));
        }

        [TestMethod]
        public void Ctor_PlayedTooFew_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("played", () => new MadeHand(HandTypes.HighCard, "2h 3d 4c 5s".ParseCards()));
        }

        [TestMethod]
        public void Ctor_PlayedTooMany_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("played", () => new MadeHand(HandTypes.HighCard, "2h 3d 4c 5s 6h 7d".ParseCards()));
        }

        [TestMethod]
        public void Ctor_AlternatesNull_ShouldSucceed()
        {
            new MadeHand(HandTypes.HighCard, HighHand, null);
        }

        [TestMethod]
        public void Ctor_Type_ShouldSet()
        {
            var actual = new MadeHand(HandTypes.ThreeOfAKind, HighHand);
            Assert.AreEqual(HandTypes.ThreeOfAKind, actual.Type);
        }

        [TestMethod]
        public void Ctor_Played_ShouldSet()
        {
            var actual = new MadeHand(HandTypes.HighCard, HighHand);
            Assert.AreEqual(HighHand.ToCardString(), actual.Played.ToCardString());
        }

        [TestMethod]
        public void Ctor_Score_ShouldBeCorrect()
        {
            var actual = new MadeHand(HandTypes.HighCard, HighHand);
            Assert.AreEqual(1108152157446, actual.Score);
        }

        [TestMethod]
        public void Ctor_SerializedNull_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("binary", "Value must be exactly 26 bytes", () => new MadeHand(null));
        }

        [TestMethod]
        public void Ctor_SerializedTooShort_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("binary", "Value must be exactly 26 bytes", () => new MadeHand(new byte[25]));
        }

        [TestMethod]
        public void Ctor_SerializedTooLong_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("binary", "Value must be exactly 26 bytes", () => new MadeHand(new byte[27]));
        }

        [TestMethod]
        public void Ctor_SerializedValid_ShouldConstruct()
        {
            var actual = new MadeHand(Convert.FromBase64String("ASEiJAAAMjM0AABDAAAAAFQAAAAAYQAAAAA="));
            Assert.AreEqual("ASEiJAAAMjM0AABDAAAAAFQAAAAAYQAAAAA=", Convert.ToBase64String(actual.Serialize()));
        }

        [TestMethod]
        public void ToString_Normal_ShouldReturnKnownValue()
        {
            var arranged = new MadeHand(HandTypes.HighCard, HighHand, CreateBasicAlternates());
            Assert.AreEqual("HighCard: 2h 3d 4c 5s 6h", arranged.ToString());
        }

        [TestMethod]
        public void Serialize_Normal_ShouldSerialize()
        {
            var arranged = new MadeHand(HandTypes.HighCard, HighHand, CreateBasicAlternates());
            Assert.AreEqual("ASEiJAAAMjM0AABDAAAAAFQAAAAAYQAAAAA=", Convert.ToBase64String(arranged.Serialize()));
        }

        private Dictionary<Card, List<Card>> CreateBasicAlternates()
        {
            return CreateAlternates("2h 3d", "2d 2s", "3c 3s");
        }

        private Dictionary<Card, List<Card>> CreateAlternates(string main, params string[] parts)
        {
            var alternates = new Dictionary<Card, List<Card>>();
            var mainCards = main.ParseCards();
            for (int i = 0; i < mainCards.Length; i++)
            {
                alternates.Add(mainCards[i], new List<Card>(parts[i].ParseCards()));
            }
            return alternates;
        }
    }
}