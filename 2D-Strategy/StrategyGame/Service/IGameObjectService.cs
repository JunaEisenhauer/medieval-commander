using System.Collections.Generic;
using StrategyGame.Model;

namespace StrategyGame.Model.IService
{
    public interface IGameObjectService : IService
    {
        IEnumerable<IEntity> GetEntities();

        void AddEntity(IEntity entity);

        void RemoveEntity(IEntity entity);

        void Reset();
    }
}
