using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace Gravical.Poker.Core.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CardTests : TestBase
    {
        private static readonly Card Simple = new Card(Face.Five, Suit.Club);

        [TestMethod]
        public void Ctor_Default_ShouldSetZeroSuitAndFace()
        {
            var actual = new Card();
            Assert.AreEqual(0, (int)actual.Suit);
            Assert.AreEqual(0, (int)actual.Face);
        }

        [TestMethod]
        public void Ctor_InvalidSuit_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("suit", () => new Card(Face.Two, (Suit)(-1)));
            ActExpectingArgumentException("suit", () => new Card(Face.Two, (Suit)0));
            ActExpectingArgumentException("suit", () => new Card(Face.Two, (Suit)5));
        }

        [TestMethod]
        public void Ctor_InvalidFace_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("face", () => new Card((Face)(-1), Suit.Heart));
            ActExpectingArgumentException("face", () => new Card((Face)0, Suit.Heart));
            ActExpectingArgumentException("face", () => new Card((Face)1, Suit.Heart));
            ActExpectingArgumentException("face", () => new Card((Face)15, Suit.Heart));
        }

        [TestMethod]
        public void Ctor_Normal_ShouldConstruct()
        {
            Assert.AreEqual(Suit.Club, Simple.Suit);
            Assert.AreEqual(Face.Five, Simple.Face);
        }

        [TestMethod]
        public void Ctor_BinaryInvalidCard_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("binary", "Card is not valid", () => new Card(0xFF));
        }

        [TestMethod]
        public void Ctor_BinaryValid_ShouldCreateCard()
        {
            var actual = new Card(0x21);
            Assert.AreEqual("2h", actual.ToString());
        }

        [TestMethod]
        public void Suit_Set_ShouldNotExist()
        {
            Assert.IsFalse(PropertySetAvailable(typeof(Card), "Suit"));
        }

        [TestMethod]
        public void Suit_Get_ShouldGet()
        {
            Assert.AreEqual(Suit.Club, Simple.Suit);
        }

        [TestMethod]
        public void Face_Set_ShouldNotExist()
        {
            Assert.IsFalse(PropertySetAvailable(typeof(Card), "Face"));
        }

        [TestMethod]
        public void Face_Get_ShouldGet()
        {
            Assert.AreEqual(Face.Five, Simple.Face);
        }

        [TestMethod]
        public void IsValid_InvalidSuit_ShouldReturnFalse()
        {
            var arrange = BinaryToStruct<Card>(new byte[] { 2, 0, 0, 0, 0, 0, 0, 0 });
            Assert.IsFalse(arrange.Suit.IsValid());
            Assert.IsTrue(arrange.Face.IsValid());
            Assert.IsFalse(arrange.IsValid);
        }

        [TestMethod]
        public void IsValid_InvalidFace_ShouldReturnFalse()
        {
            var arrange = BinaryToStruct<Card>(new byte[] { 0, 0, 0, 0, 1, 0, 0, 0 });
            Assert.IsTrue(arrange.Suit.IsValid());
            Assert.IsFalse(arrange.Face.IsValid());
            Assert.IsFalse(arrange.IsValid);
        }

        [TestMethod]
        public void IsValid_Valid_ShouldReturnTrue()
        {
            var arrange = BinaryToStruct<Card>(new byte[] { 2, 0, 0, 0, 1, 0, 0, 0 });
            Assert.IsTrue(arrange.Suit.IsValid());
            Assert.IsTrue(arrange.Face.IsValid());
            Assert.IsTrue(arrange.IsValid);
        }

        [TestMethod]
        public void Equals_ObjectNull_ShouldReturnFalse()
        {
            Assert.IsTrue(Simple.Equals(new Card(Simple.Face, Simple.Suit)));
        }

        [TestMethod]
        public void Equals_ObjectNotCard_ShouldReturnFalse()
        {
            Assert.IsFalse(Simple.Equals(new object()));
        }

        [TestMethod]
        public void Equals_ObjectDifferntSuit_ShouldReturnFalse()
        {
            Assert.IsFalse(Simple.Equals(new Card(Simple.Face, Suit.Spade)));
        }

        [TestMethod]
        public void Equals_ObjectDifferntFace_ShouldReturnFalse()
        {
            Assert.IsFalse(Simple.Equals(new Card(Face.Ten, Simple.Suit)));
        }

        [TestMethod]
        public void Equals_ObjectSame_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.Equals(new Card(Simple.Face, Simple.Suit)));
        }

        [TestMethod]
        public void GetHashCode_Object_ShouldReturnKnownValue()
        {
            Assert.AreEqual(83, Simple.GetHashCode());
        }

        [TestMethod]
        public void ToBinary_Normal_ShouldReturnKnownValue()
        {
            Assert.AreEqual((byte)83, Simple.ToBinary());
        }

        [TestMethod]
        public void ToString_Zero_ShouldReturnKnownValue()
        {
            Assert.AreEqual("Invalid", new Card().ToString());
        }

        [TestMethod]
        public void ToString_Default_ShouldReturnKnownValue()
        {
            Assert.AreEqual("5c", Simple.ToString());
        }

        [TestMethod]
        public void ToString_FormatShort_ShouldReturnKnownValue()
        {
            Assert.AreEqual("5c", Simple.ToString("s"));
        }

        [TestMethod]
        public void ToString_FormatLong_ShouldReturnKnownValue()
        {
            Assert.AreEqual("Five of Club", Simple.ToString("l"));
        }

        [TestMethod]
        public void ToString_FormatUnknown_ShouldReturnKnownValue()
        {
            Assert.AreEqual("5c", Simple.ToString("?"));
        }

        [TestMethod]
        public void Key_Zero_ShouldReturnZero()
        {
            Assert.AreEqual((byte)0, new Card().Key);
        }

        [TestMethod]
        public void Key_InvalidFace_ShouldReturnZero()
        {
            var arrange = BinaryToStruct<Card>(new byte[] { 15, 0, 0, 0, 1, 0, 0, 0 });
            Assert.IsTrue(arrange.Suit.IsValid());
            Assert.IsFalse(arrange.Face.IsValid());
            Assert.AreEqual((byte)0, arrange.Key);
        }

        [TestMethod]
        public void Key_InvalidSuit_ShouldReturnZero()
        {
            var arrange = BinaryToStruct<Card>(new byte[] { 2, 0, 0, 0, 5, 0, 0, 0 });
            Assert.IsFalse(arrange.Suit.IsValid());
            Assert.IsTrue(arrange.Face.IsValid());
            Assert.AreEqual((byte)0, arrange.Key);
        }

        [TestMethod]
        public void Key_Valid_ShouldReturnValid()
        {
            Assert.AreEqual((byte)227, new Card(Face.Ace, Suit.Club).Key);
            Assert.AreEqual((byte)161, new Card(Face.Ten, Suit.Heart).Key);
            Assert.AreEqual((byte)66, new Card(Face.Four, Suit.Diamond).Key);
            Assert.AreEqual((byte)36, new Card(Face.Two, Suit.Spade).Key);
        }
    }
}