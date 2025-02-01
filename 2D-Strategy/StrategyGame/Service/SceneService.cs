using System;
using StrategyGame.Model.IService;

namespace StrategyGame.Service
{
    public class SceneService : ISceneService
    {
        private Action<string> changeScene;

        public SceneService(Action<string> changeScene)
        {
            this.changeScene = changeScene;
        }

        public void ChangeScene(string scene)
        {
            changeScene.Invoke(scene);
        }
    }
}
