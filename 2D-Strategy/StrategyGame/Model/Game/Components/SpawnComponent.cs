using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using StrategyGame.Model.IService;
using StrategyGame.Service;

namespace StrategyGame.Model.Game.Components
{
    public class SpawnComponent : IComponent
    {
        public IEntity Parent { get; }

        public Vector2 Spawnpoint => Parent.Position;

        public float SpawnSpeed { get; }

        private float lastSpawn;

        public SpawnComponent(IEntity parent, float spawnSpeed)
        {
            Parent = parent;
            parent?.AddComponent(this);
            SpawnSpeed = spawnSpeed;
        }

        public void Update()
        {
            if (Parent.Scene.ServiceProvider.GetService<ITimeService>().TimeScale == 0)
                return;

            var serviceProvider = Parent.Scene.ServiceProvider;

            var timeService = serviceProvider.GetService<ITimeService>();
            if (lastSpawn + SpawnSpeed > timeService.AbsoluteTime)
                return;
            lastSpawn = timeService.AbsoluteTime;

            var random = serviceProvider.GetService<IRandomService>().GetRandomInt(3);
            var gameEntityFactory = (GameEntityFactory)Parent.Scene.EntityFactory;
            var owner = Parent.GetComponent<OwnerComponent>().Owner;
            IEntity entity = null;
            switch (random)
            {
                case 0:
                    entity = gameEntityFactory.CreateArcher(Spawnpoint, owner, 0);
                    break;
                case 1:
                    entity = gameEntityFactory.CreateKnight(Spawnpoint, owner, 0);
                    break;
                case 2:
                    entity = gameEntityFactory.CreateCatapult(Spawnpoint, owner, 0);
                    break;
                default:
                    break;
            }

            if (entity == null)
                return;

            new MoveRandomComponent(entity);
        }
    }
}
