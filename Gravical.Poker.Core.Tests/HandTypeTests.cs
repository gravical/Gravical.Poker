using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;


namespace Gravical.Poker.Core.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HandTypesTests : TestBase
    {
        [TestMethod]
        public void Enum_Values_ShouldContainOnlyKnownValues()
        {
            var names = Enum.GetNames(typeof(HandTypes));
            Assert.AreEqual(10, names.Length);
            Assert.AreEqual("HighCard", names[0]);
            Assert.AreEqual("Pair", names[1]);
            Assert.AreEqual("TwoPair", names[2]);
            Assert.AreEqual("ThreeOfAKind", names[3]);
            Assert.AreEqual("Straight", names[4]);
            Assert.AreEqual("Flush", names[5]);
            Assert.AreEqual("FullHouse", names[6]);
            Assert.AreEqual("FourOfAKind", names[7]);
            Assert.AreEqual("StraightFlush", names[8]);
            Assert.AreEqual("RoyalFlush", names[9]);
        }
    }
}