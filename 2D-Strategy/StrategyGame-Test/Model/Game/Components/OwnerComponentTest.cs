using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using StrategyGame.Model;
using StrategyGame.Model.Game.Components;

namespace StrategyGame_Test.Model.Game.Components
{
    [TestClass]
    public class OwnerComponentTest
    {
        [TestMethod]
        public void TestOwner0()
        {
            IEntity entity = new Entity(null, Vector2.Zero, Vector2.Zero, false);
            var ownerComponent = new OwnerComponent(entity, 0);

            entity.Update();

            Assert.IsTrue(entity.HasComponent<OwnerComponent>());
            Assert.AreSame(ownerComponent, entity.GetComponent<OwnerComponent>());
            Assert.AreSame(entity, entity.GetComponent<OwnerComponent>().Parent);

            Assert.AreEqual(0, ownerComponent.Owner);
        }

        public void TestOwner1()
        {
            IEntity entity = new Entity(null, Vector2.Zero, Vector2.Zero, false);
            var ownerComponent = new OwnerComponent(entity, 1);

            entity.Update();

            Assert.AreEqual(1, ownerComponent.Owner);
        }

        public void TestOwner10()
        {
            IEntity entity = new Entity(null, Vector2.Zero, Vector2.Zero, false);
            var ownerComponent = new OwnerComponent(entity, 10);

            entity.Update();

            Assert.AreEqual(10, ownerComponent.Owner);
        }
    }
}
