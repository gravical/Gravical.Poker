using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;


namespace Gravical.Poker.Core.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SuitTests : TestBase
    {
        [TestMethod]
        public void Enum_Values_ShouldContainOnlyKnownValues()
        {
            var names = Enum.GetNames(typeof(Suit));
            Assert.AreEqual(4, names.Length);
            Assert.AreEqual("Heart", names[0]);
            Assert.AreEqual("Diamond", names[1]);
            Assert.AreEqual("Club", names[2]);
            Assert.AreEqual("Spade", names[3]);
        }

        [TestMethod]
        public void Enum_Heart_ShouldBe1()
        {
            Assert.AreEqual(1, (int)Suit.Heart);
        }

        [TestMethod]
        public void Enum_Diamond_ShouldBe2()
        {
            Assert.AreEqual(2, (int)Suit.Diamond);
        }

        [TestMethod]
        public void Enum_Club_ShouldBe3()
        {
            Assert.AreEqual(3, (int)Suit.Club);
        }

        [TestMethod]
        public void Enum_Spade_ShouldBe4()
        {
            Assert.AreEqual(4, (int)Suit.Spade);
        }
    }
}