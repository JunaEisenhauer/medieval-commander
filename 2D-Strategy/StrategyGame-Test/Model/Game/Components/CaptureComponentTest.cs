using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using StrategyGame.Model;
using StrategyGame.Model.Game.Components;

namespace StrategyGame_Test.Model.Game.Components
{
    [TestClass]
    public class CaptureComponentTest
    {
        [TestMethod]
        public void TestCapture()
        {
            IEntity entity = new Entity(null, Vector2.Zero, Vector2.Zero, false);
            var captureComponent = new CaptureComponent(entity);

            entity.Update();

            Assert.IsTrue(entity.HasComponent<CaptureComponent>());
            Assert.AreSame(captureComponent, entity.GetComponent<CaptureComponent>());
            Assert.AreSame(entity, entity.GetComponent<CaptureComponent>().Parent);
        }
    }
}
