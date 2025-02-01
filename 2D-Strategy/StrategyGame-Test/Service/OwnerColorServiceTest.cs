using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGame.Service;

namespace StrategyGame_Test.Service
{
    [TestClass]
    public class OwnerColorServiceTest
    {
        [TestMethod]
        public void TestColor()
        {
            var service = new OwnerColorService();

            Assert.AreEqual(Color.Blue, service.GetOwnerColor(0));
            Assert.AreEqual(Color.Red, service.GetOwnerColor(1));
            Assert.AreEqual(Color.White, service.GetOwnerColor(2));
            Assert.AreEqual(Color.White, service.GetOwnerColor(10));
        }
    }
}
