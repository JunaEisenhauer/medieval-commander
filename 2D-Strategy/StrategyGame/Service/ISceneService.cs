using System;

namespace StrategyGame.Model.IService
{
    public interface ISceneService : IService
    {
        void ChangeScene(string scene);
    }
}
