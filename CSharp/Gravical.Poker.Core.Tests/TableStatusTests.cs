using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;


namespace Gravical.Poker.Core.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TableStatusTests : TestBase
    {
        [TestMethod]
        public void Enum_Values_ShouldContainOnlyKnownValues()
        {
            var names = Enum.GetNames(typeof(TableStatus));
            Assert.AreEqual(4, names.Length);
            Assert.AreEqual("BeforeFlop", names[0]);
            Assert.AreEqual("BeforeTurn", names[1]);
            Assert.AreEqual("BeforeRiver", names[2]);
            Assert.AreEqual("Complete", names[3]);
        }
    }
}