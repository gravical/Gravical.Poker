using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;


namespace Gravical.Poker.Core.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BettingRoundStatusTests : TestBase
    {
        [TestMethod]
        public void Enum_Values_ShouldContainOnlyKnownValues()
        {
            var names = Enum.GetNames(typeof(BettingRoundStatus));
            Assert.AreEqual(5, names.Length);
            Assert.AreEqual("Unopened", names[0]);
            Assert.AreEqual("Folded", names[1]);
            Assert.AreEqual("Checked", names[2]);
            Assert.AreEqual("Called", names[3]);
            Assert.AreEqual("Raised", names[4]);
        }
    }
}