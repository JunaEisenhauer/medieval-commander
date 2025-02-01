using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using StrategyGame.Model;
using StrategyGame.Model.Game.Components;

namespace StrategyGame_Test.Model.Game.Components
{
    [TestClass]
    public class MoveComponentTest
    {
        [TestMethod]
        public void TestMoveComponent()
        {
            IEntity entity = new Entity(null, Vector2.Zero, Vector2.Zero, false);
            var moveComponent = new MoveComponent(entity, 10f);

            entity.Update();

            Assert.IsTrue(entity.HasComponent<MoveComponent>());
            Assert.AreSame(moveComponent, entity.GetComponent<MoveComponent>());
            Assert.AreSame(entity, entity.GetComponent<MoveComponent>().Parent);

            Assert.AreEqual(10f, moveComponent.Speed);
        }
        

        [TestMethod]
        public void TestMove()
        {
            IEntity entity = new Entity(null, Vector2.Zero, Vector2.Zero, false);
            var moveComponent = new MoveComponent(entity, 10f);

            entity.Update();

            Assert.IsTrue(entity.HasComponent<MoveComponent>());
            var move = entity.GetComponent<MoveComponent>();

            Assert.IsFalse(moveComponent.Moving);
            Assert.IsNull(moveComponent.Destination);

            move.Moving = true;

            Assert.IsTrue(move.Moving);
        }
    }
}
