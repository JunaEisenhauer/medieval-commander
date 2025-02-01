using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using StrategyGame.Model;
using StrategyGame.Model.Game.Components;

namespace StrategyGame_Test.Model.Game.Components
{
    [TestClass]
    public class RelativeComponentTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            IEntity follow = new Entity(null, new Vector2(10f), Vector2.Zero, false);

            IEntity entity = new Entity(null, new Vector2(2f), Vector2.Zero, false);
            var relativeComponent = new RelativeComponent(entity, follow, new Vector2(5f));

            entity.Update();

            Assert.IsTrue(entity.HasComponent<RelativeComponent>());
            Assert.AreSame(relativeComponent, entity.GetComponent<RelativeComponent>());
            Assert.AreSame(entity, entity.GetComponent<RelativeComponent>().Parent);

            var relative = entity.GetComponent<RelativeComponent>();

            Assert.AreSame(follow, relative.Follow);
            Assert.AreEqual(new Vector2(5f), relative.Offset);

            entity.Update();

            Assert.AreEqual(new Vector2(15f), entity.Position);


            follow.Position = new Vector2(100f);
            entity.Update();

            Assert.AreEqual(new Vector2(105f), entity.Position);
        }
    }
}
