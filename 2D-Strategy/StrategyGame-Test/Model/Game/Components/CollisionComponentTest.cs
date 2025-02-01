using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using StrategyGame.Model;
using StrategyGame.Model.Game.Components;

namespace StrategyGame_Test.Model.Game.Components
{
    [TestClass]
    public class CollisionComponentTest
    {
        [TestMethod]
        public void TestCollision()
        {
            IEntity entity = new Entity(null, Vector2.Zero, Vector2.Zero, false);
            var collisionComponent = new CollisionComponent(entity);

            entity.Update();

            Assert.IsTrue(entity.HasComponent<CollisionComponent>());
            Assert.AreSame(collisionComponent, entity.GetComponent<CollisionComponent>());
            Assert.AreSame(entity, entity.GetComponent<CollisionComponent>().Parent);
        }
    }
}
