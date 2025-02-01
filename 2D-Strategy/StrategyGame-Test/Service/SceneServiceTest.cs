using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGame.Service;

namespace StrategyGame_Test.Service
{
    [TestClass]
    public class SceneServiceTest
    {
        [TestMethod]
        public void TestScene()
        {
            var service = new SceneService(sceneName =>
            {
                Assert.AreEqual("TestScene", sceneName);
            });

            service.ChangeScene("TestScene");
        }

        [TestMethod]
        public void TestMultiScenes()
        {
            var i = 0;

            var service = new SceneService(sceneName =>
            {
                Assert.AreEqual("TestScene" + i, sceneName);

                i++;
            });

            for (var j = 0; j < 100; j++)
            {
                service.ChangeScene("TestScene" + j);
            }
        }
    }
}
