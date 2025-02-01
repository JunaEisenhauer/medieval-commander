using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StrategyGame.Model;
using StrategyGame.Model.IService;

namespace StrategyGame.Service
{
    public class GameObjectService : IGameObjectService
    {
        private List<IEntity> entities;

        public GameObjectService()
        {
            entities = new List<IEntity>();
        }

        public void AddEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        public void Reset()
        {
            entities = new List<IEntity>();
        }

        public IEnumerable<IEntity> GetEntities()
        {
            return entities;
        }
    }
}
