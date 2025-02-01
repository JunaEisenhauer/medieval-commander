using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using StrategyGame.Model;
using StrategyGame.Model.Game.Components;

namespace StrategyGame_Test.Model.Game.Components
{
    [TestClass]
    public class MoveRandomComponentTest
    {
        [TestMethod]
        public void TestMoveRandom()
        {
            IEntity entity = new Entity(null, Vector2.Zero, Vector2.Zero, false);
            var moveRandomComponent = new MoveRandomComponent(entity);

            entity.Update();

            Assert.IsTrue(entity.HasComponent<MoveRandomComponent>());
            Assert.AreSame(moveRandomComponent, entity.GetComponent<MoveRandomComponent>());
            Assert.AreSame(entity, entity.GetComponent<MoveRandomComponent>().Parent);
        }
    }
}
