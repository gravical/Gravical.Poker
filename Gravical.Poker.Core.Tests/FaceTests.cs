using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Gravical.Poker.Core.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class FaceTests
    {
        [TestMethod]
        public void Enum_Values_ShouldContainOnlyKnownValues()
        {
            var names = Enum.GetNames(typeof(Face));
            Assert.AreEqual(13, names.Length);
            Assert.AreEqual("Two", names[0]);
            Assert.AreEqual("Three", names[1]);
            Assert.AreEqual("Four", names[2]);
            Assert.AreEqual("Five", names[3]);
            Assert.AreEqual("Six", names[4]);
            Assert.AreEqual("Seven", names[5]);
            Assert.AreEqual("Eight", names[6]);
            Assert.AreEqual("Nine", names[7]);
            Assert.AreEqual("Ten", names[8]);
            Assert.AreEqual("Jack", names[9]);
            Assert.AreEqual("Queen", names[10]);
            Assert.AreEqual("King", names[11]);
            Assert.AreEqual("Ace", names[12]);
        }

        [TestMethod]
        public void Enum_Two_ShouldBe2()
        {
            Assert.AreEqual(2, (int)Face.Two);
        }

        [TestMethod]
        public void Enum_Three_ShouldBe3()
        {
            Assert.AreEqual(3, (int)Face.Three);
        }

        [TestMethod]
        public void Enum_Four_ShouldBe4()
        {
            Assert.AreEqual(4, (int)Face.Four);
        }

        [TestMethod]
        public void Enum_Five_ShouldBe5()
        {
            Assert.AreEqual(5, (int)Face.Five);
        }

        [TestMethod]
        public void Enum_Six_ShouldBe6()
        {
            Assert.AreEqual(6, (int)Face.Six);
        }

        [TestMethod]
        public void Enum_Seven_ShouldBe7()
        {
            Assert.AreEqual(7, (int)Face.Seven);
        }

        [TestMethod]
        public void Enum_Eight_ShouldBe8()
        {
            Assert.AreEqual(8, (int)Face.Eight);
        }

        [TestMethod]
        public void Enum_Nine_ShouldBe9()
        {
            Assert.AreEqual(9, (int)Face.Nine);
        }

        [TestMethod]
        public void Enum_Ten_ShouldBe10()
        {
            Assert.AreEqual(10, (int)Face.Ten);
        }

        [TestMethod]
        public void Enum_Jack_ShouldBe11()
        {
            Assert.AreEqual(11, (int)Face.Jack);
        }

        [TestMethod]
        public void Enum_Queen_ShouldBe12()
        {
            Assert.AreEqual(12, (int)Face.Queen);
        }

        [TestMethod]
        public void Enum_King_ShouldBe13()
        {
            Assert.AreEqual(13, (int)Face.King);
        }

        [TestMethod]
        public void Enum_Ace_ShouldBe14()
        {
            Assert.AreEqual(14, (int)Face.Ace);
        }
    }
}