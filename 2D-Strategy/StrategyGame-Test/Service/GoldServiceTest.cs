using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGame.Service;

namespace StrategyGame_Test.Service
{
    [TestClass]
    public class GoldServiceTest
    {
        [TestMethod]
        public void TestInitialGold()
        {
            var service = new GoldService();

            Assert.AreEqual(0, service.GetGold(0));
            Assert.AreEqual(0, service.GetGold(1));
            Assert.AreEqual(0, service.GetGold(10));
        }

        [TestMethod]
        public void TestGold()
        {
            var service = new GoldService();

            service.AddGold(0, 1);

            Assert.AreEqual(1, service.GetGold(0));

            service.AddGold(0, 3);

            Assert.AreEqual(4, service.GetGold(0));
            Assert.AreEqual(0, service.GetGold(1));

            service.AddGold(1, 1);

            Assert.AreEqual(4, service.GetGold(0));
            Assert.AreEqual(1, service.GetGold(1));


            service.RemoveGold(0, 2);

            Assert.AreEqual(2, service.GetGold(0));
            Assert.AreEqual(1, service.GetGold(1));

            service.RemoveGold(0, -10);

            Assert.AreEqual(12, service.GetGold(0));
            Assert.AreEqual(1, service.GetGold(1));

            service.RemoveGold(1, 1);

            Assert.AreEqual(12, service.GetGold(0));
            Assert.AreEqual(0, service.GetGold(1));

            service.RemoveGold(1, 1);

            Assert.AreEqual(12, service.GetGold(0));
            Assert.AreEqual(0, service.GetGold(1));

            service.RemoveGold(0, 100);

            Assert.AreEqual(0, service.GetGold(0));
            Assert.AreEqual(0, service.GetGold(1));
        }

        [TestMethod]
        public void TestGoldReset()
        {
            var service = new GoldService();


            service.AddGold(0, 3);
            service.AddGold(1, 12);

            service.Reset();

            Assert.AreEqual(0, service.GetGold(0));
            Assert.AreEqual(0, service.GetGold(1));
            Assert.AreEqual(0, service.GetGold(2));
        }
    }
}
