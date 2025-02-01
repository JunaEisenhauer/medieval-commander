using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGame.Service;

namespace StrategyGame_Test.Service
{
    [TestClass]
    public class GameStateServiceTest
    {
        [TestMethod]
        public void TestRunningWon()
        {
            var service = new GameStateService();

            Assert.IsFalse(service.Running);
            Assert.IsFalse(service.Won);

            service.Running = true;

            Assert.IsTrue(service.Running);
            Assert.IsFalse(service.Won);

            service.Won = true;

            Assert.IsTrue(service.Running);
            Assert.IsTrue(service.Won);

            service.Running = false;
            service.Won = false;

            Assert.IsFalse(service.Running);
            Assert.IsFalse(service.Won);
        }
    }
}
