using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using StrategyGame.Model;
using StrategyGame.Model.Game.Components;

namespace StrategyGame_Test.Model.Game.Components
{
    [TestClass]
    public class HealComponentTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            IEntity entity = new Entity(null, Vector2.Zero, Vector2.Zero, false);
            var healComponent = new HealComponent(entity);

            entity.Update();

            Assert.IsTrue(entity.HasComponent<HealComponent>());
            Assert.AreSame(healComponent, entity.GetComponent<HealComponent>());
            Assert.AreSame(entity, entity.GetComponent<HealComponent>().Parent);
        }
    }
}
