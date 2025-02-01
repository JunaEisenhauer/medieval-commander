using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using StrategyGame.Model;
using StrategyGame.Model.Game.Components;

namespace StrategyGame_Test.Model.Game.Components
{
    [TestClass]
    public class HealthComponentTest
    {
        [TestMethod]
        public void TestHealth()
        {
            IEntity entity = new Entity(null, Vector2.Zero, Vector2.Zero, false);
            var healthComponent = new HealthComponent(entity, 100, 0);

            entity.Update();

            Assert.IsTrue(entity.HasComponent<HealthComponent>());

            var healthComp = entity.GetComponent<HealthComponent>();

            Assert.AreSame(healthComponent, healthComp);
            Assert.AreSame(entity, healthComp.Parent);

            Assert.AreEqual(100, healthComp.Health);

            healthComp.Damage(10, entity);

            Assert.AreEqual(90, healthComp.Health);
            Assert.IsFalse(healthComp.Dead);

            healthComp.Damage(100, entity);

            Assert.AreEqual(0, healthComp.Health);
            Assert.IsTrue(healthComp.Dead);

            Assert.AreEqual(100, healthComp.MaxHealth);
            Assert.AreEqual(0, healthComp.ArmyType);
        }
    }
}
