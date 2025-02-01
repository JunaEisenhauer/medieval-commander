using System;
using OpenTK;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StrategyGame.Model.Game
{
    [TestClass]
    public class CollisionTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Archer archer = new Archer(PlayerType.Player, new Vector2(0f, 0f));
            Archer archer2 = new Archer(PlayerType.Player, new Vector2(0f, 0f));

            Assert.IsTrue(archer2.HasInRange(archer));
        }

        [TestMethod]
        public void TestMethod2()
        {
            Archer archer = new Archer(PlayerType.Player, new Vector2(0f, 0f));
            Archer archer2 = new Archer(PlayerType.Player, new Vector2(archer.Radius + archer.Range, 0f));

            Assert.IsFalse(archer2.HasInRange(archer));
        }

        [TestMethod]
        public void TestMethod3()
        {
            Archer archer = new Archer(PlayerType.Player, new Vector2(0f, 0f));
            Archer archer2 = new Archer(PlayerType.Player, new Vector2(archer.Radius + archer.Range - 0.1f, 0f));

            Assert.IsTrue(archer2.HasInRange(archer));
        }
    }
}
