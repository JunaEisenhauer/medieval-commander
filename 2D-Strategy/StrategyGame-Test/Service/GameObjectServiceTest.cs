using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using StrategyGame.Model;
using StrategyGame.Model.IService;
using StrategyGame.Service;

namespace StrategyGame_Test.Service
{
    [TestClass]
    public class GameObjectServiceTest
    {
        [TestMethod]
        public void TestIEnumerable()
        {
            var service = new GameObjectService();

            Assert.IsNotNull(service.GetEntities());

            Assert.AreEqual(0, service.GetEntities().Count());
        }

        [TestMethod]
        public void TestEntities()
        {
            var service = new GameObjectService();

            IEntity entity1 = new TestEntity(service, false);

            Assert.AreEqual(0, service.GetEntities().Count());

            IEntity entity2 = new TestEntity(service, true);

            Assert.AreEqual(1, service.GetEntities().Count());

            var enumerator = service.GetEntities().GetEnumerator();
            enumerator.MoveNext();

            Assert.AreNotSame(entity1, enumerator.Current);

            Assert.AreSame(entity2, enumerator.Current);

            IEntity entity3 = new TestEntity(service, true);

            Assert.AreEqual(2, service.GetEntities().Count());
        }
    }


    class TestEntity : IEntity
    {

        public TestEntity(IGameObjectService gameObjectService, bool isGameObject)
        {
            if (isGameObject)
                gameObjectService.AddEntity(this);
        }

        public void Update()
        {
        }

        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public IScene Scene { get; }
        public void AddChild(IEntity entity)
        {
        }

        public void RemoveChild(IEntity entity)
        {
        }

        public void AddComponent(IComponent component)
        {
        }

        public void RemoveComponent<T>() where T : IComponent
        {
        }

        public T GetComponent<T>() where T : IComponent
        {
            return default(T);
        }

        public bool HasComponent<T>() where T : IComponent
        {
            return false;
        }

        public void Destroy()
        {
        }
    }
}
