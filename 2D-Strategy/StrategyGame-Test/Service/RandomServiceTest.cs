using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGame.Service;

namespace StrategyGame_Test.Service
{
    [TestClass]
    public class RandomServiceTest
    {
        [TestMethod]
        public void TestRandomInt()
        {
            var service = new RandomService();

            Assert.IsTrue(service.GetRandomInt(1) < 1);
            Assert.IsTrue(service.GetRandomInt(1) >= 0);

            Assert.IsTrue(service.GetRandomInt(10) < 10);
            Assert.IsTrue(service.GetRandomInt(10) >= 0);

            Assert.IsFalse(service.GetRandomInt(10) >= 10);
            Assert.IsFalse(service.GetRandomInt(10) < 0);
        }

        [TestMethod]
        public void TestRandomFloat()
        {
            var service = new RandomService();

            Assert.IsTrue(service.GetRandomFloat(1) < 1);
            Assert.IsTrue(service.GetRandomFloat(1) >= 0);

            Assert.IsTrue(service.GetRandomFloat(10) < 10);
            Assert.IsTrue(service.GetRandomFloat(10) >= 0);

            Assert.IsFalse(service.GetRandomFloat(10) >= 10);
            Assert.IsFalse(service.GetRandomFloat(10) < 0);

            Assert.IsTrue(service.GetRandomFloat(1.1f) < 1.1f);
            Assert.IsTrue(service.GetRandomFloat(10.5f) < 10.5f);
            Assert.IsTrue(service.GetRandomFloat(10.5f) >= 0);
        }
    }
}
