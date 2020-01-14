using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gravical.Poker.Core.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PocketTests : TestBase
    {
        private Deck Deck;
        private Pocket Simple;

        [TestInitialize]
        public new void Initialize()
        {
            base.Initialize();
            Deck = new Deck();
            Simple = new Pocket(new byte[] { 0x21, 0x84 });
        }

        [TestMethod]
        public void CardA_Set_ShouldNotExist()
        {
            Assert.IsFalse(PropertySetAvailable(typeof(Pocket), "CardA"));
        }

        [TestMethod]
        public void CardA_Get_ShouldGet()
        {
            Assert.IsTrue(Simple.CardA.Suit.IsValid());
            Assert.IsTrue(Simple.CardA.Face.IsValid());
        }

        [TestMethod]
        public void CardB_Set_ShouldNotExist()
        {
            Assert.IsFalse(PropertySetAvailable(typeof(Pocket), "CardB"));
        }

        [TestMethod]
        public void CardB_Get_ShouldGet()
        {
            Assert.IsTrue(Simple.CardB.Suit.IsValid());
            Assert.IsTrue(Simple.CardB.Face.IsValid());
        }

        [TestMethod]
        public void Ctor_NullDeck_ShouldThrowArgumentNullException()
        {
            ActExpectingArgumentNullException("deck", () => new Pocket(null as Deck));
        }

        [TestMethod]
        public void Ctor_Normal_ShouldSucceed()
        {
            new Pocket(Deck);
        }

        [TestMethod]
        public void Ctor_Normal_ShouldSetCardA()
        {
            var actual = new Pocket(Deck);
            Assert.IsTrue(actual.CardA.Suit.IsValid());
            Assert.IsTrue(actual.CardA.Face.IsValid());
        }

        [TestMethod]
        public void Ctor_Normal_ShouldSetCardB()
        {
            var actual = new Pocket(Deck);
            Assert.IsTrue(actual.CardB.Suit.IsValid());
            Assert.IsTrue(actual.CardB.Face.IsValid());
        }

        [TestMethod]
        public void Ctor_Normal_ShouldSetDistinctCards()
        {
            var actual = new Pocket(Deck);
            Assert.IsFalse(actual.CardB.Equals(actual.CardA));
        }

        [TestMethod]
        public void Ctor_BinaryNull_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("binary", "Value must be exactly 2 bytes", () => new Pocket((byte[])null));
        }

        [TestMethod]
        public void Ctor_BinaryEmpty_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("binary", "Value must be exactly 2 bytes", () => new Pocket(new byte[0]));
        }

        [TestMethod]
        public void Ctor_BinaryTooShort_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("binary", "Value must be exactly 2 bytes", () => new Pocket(new byte[] { 0x21 }));
        }

        [TestMethod]
        public void Ctor_BinaryTooLong_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("binary", "Value must be exactly 2 bytes", () => new Pocket(new byte[] { 0x21, 0x31, 0x41 }));
        }

        [TestMethod]
        public void Ctor_BinaryInvalidCard_ShouldThrowArgumentException()
        {
            ActExpectingArgumentException("binary", "Card is not valid", () => new Pocket(new byte[] { 0x21, 0xFF }));
        }

        [TestMethod]
        public void Ctor_BinaryValid_ShouldCreate()
        {
            var actual = new Pocket(new byte[] { 0x21, 0x84 });
            Assert.AreEqual("2h", actual.CardA.ToString());
            Assert.AreEqual("8s", actual.CardB.ToString());
        }

        [TestMethod]
        public void ToArray_Normal_ShouldReturnInOrder()
        {
            var arranged = Simple.ToArray();
            Assert.AreEqual(Simple.CardA, arranged[0]);
            Assert.AreEqual(Simple.CardB, arranged[1]);
        }

        private static readonly Regex PocketStringPattern = new Regex(@"^[2-9TJQKA][hcds] [2-9TJQKA][hcds]$", RegexOptions.Compiled);

        [TestMethod]
        public void ToString_Normal_ShouldReturnGoodValue()
        {
            Console.WriteLine(Simple.ToString());
            Assert.IsTrue(PocketStringPattern.Match(Simple.ToString()).Success);
        }

        [TestMethod]
        public void IsTrumpedByPocket_Null_ShouldThrowArgumentNullException()
        {
            ActExpectingArgumentNullException("by", () => Simple.IsTrumped(null as Pocket));
        }

        [TestMethod]
        public void IsTrumpedByPocket_CardAEqualsCardA_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.IsTrumped(new Pocket(new byte[] { 0x21, 0x31 })));
        }

        [TestMethod]
        public void IsTrumpedByPocket_CardAEqualsCardB_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.IsTrumped(new Pocket(new byte[] { 0x84, 0x31 })));
        }

        [TestMethod]
        public void IsTrumpedByPocket_CardBEqualsCardA_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.IsTrumped(new Pocket(new byte[] { 0x31, 0x21 })));
        }

        [TestMethod]
        public void IsTrumpedByPocket_CardBEqualsCardB_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.IsTrumped(new Pocket(new byte[] { 0x31, 0x84 })));
        }

        [TestMethod]
        public void IsTrumpedByPocket_No_ShouldReturnFalse()
        {
            Assert.IsFalse(Simple.IsTrumped(new Pocket(new byte[] { 0x31, 0x41 })));
        }

        private TableFinal MakeTable(string value)
        {
            var cards = value.Split(new char[] { ' ' }).Select(_ => _.ParseCard().Value).ToArray();
            return new TableFinal(cards[0], cards[1], cards[2], cards[3], cards[4]);
        }

        [TestMethod]
        public void IsTrumpedByTable_Null_ShouldThrowArgumentNullException()
        {
            ActExpectingArgumentNullException("by", () => Simple.IsTrumped(null as TableFinal));
        }

        [TestMethod]
        public void IsTrumpedByTable_CardAEqualsFirst_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.IsTrumped(MakeTable("2h 3c 4s 5h 6d")));
        }

        [TestMethod]
        public void IsTrumpedByTable_CardAEqualsSecond_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.IsTrumped(MakeTable("2d 2h 4s 5h 6d")));
        }

        [TestMethod]
        public void IsTrumpedByTable_CardAEqualsThird_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.IsTrumped(MakeTable("2d 3c 2h 5h 6d")));
        }

        [TestMethod]
        public void IsTrumpedByTable_CardAEqualsTurn_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.IsTrumped(MakeTable("2d 3c 4s 2h 6d")));
        }

        [TestMethod]
        public void IsTrumpedByTable_CardAEqualsRiver_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.IsTrumped(MakeTable("2d 3c 4s 5h 2h")));
        }

        [TestMethod]
        public void IsTrumpedByTable_CardBEqualsFirst_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.IsTrumped(MakeTable("8s 3c 4s 5h 6d")));
        }

        [TestMethod]
        public void IsTrumpedByTable_CardBEqualsSecond_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.IsTrumped(MakeTable("2d 8s 4s 5h 6d")));
        }

        [TestMethod]
        public void IsTrumpedByTable_CardBEqualsThird_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.IsTrumped(MakeTable("2d 3c 8s 5h 6d")));
        }

        [TestMethod]
        public void IsTrumpedByTable_CardBEqualsTurn_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.IsTrumped(MakeTable("2d 3c 4s 8s 6d")));
        }

        [TestMethod]
        public void IsTrumpedByTable_CardBEqualsRiver_ShouldReturnTrue()
        {
            Assert.IsTrue(Simple.IsTrumped(MakeTable("2d 3c 4s 5h 8s")));
        }

        [TestMethod]
        public void IsTrumpedByTable_No_ShouldReturnFalse()
        {
            Assert.IsFalse(Simple.IsTrumped(MakeTable("2d 3c 4s 5h 6d")));
        }

        [TestMethod]
        public void ToBinary_Normal_ShouldReturnCorrectBinary()
        {
            var arrange = new byte[] { 0x21, 0x84 };
            Assert.IsTrue(new Pocket(arrange).ToBinary().SequenceEqual(arrange));
        }
    }
}