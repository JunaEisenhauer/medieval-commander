using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using StrategyGame.Model;
using StrategyGame.Model.Game.Components;

namespace StrategyGame_Test.Model.Game.Components
{
    [TestClass]
    public class CaptureGoldComponentTest
    {
        [TestMethod]
        public void TestCaptureGold()
        {
            IEntity entity = new Entity(null, Vector2.Zero, Vector2.Zero, false);
            var captureGoldComponent = new CaptureGoldComponent(entity, 100);

            entity.Update();

            Assert.IsTrue(entity.HasComponent<CaptureGoldComponent>());
            Assert.AreSame(captureGoldComponent, entity.GetComponent<CaptureGoldComponent>());
            Assert.AreSame(entity, entity.GetComponent<CaptureGoldComponent>().Parent);

            Assert.AreEqual(100, captureGoldComponent.GoldValue);
        }
    }
}
