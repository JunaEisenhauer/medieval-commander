using System;
using System.Collections.Generic;
using OpenTK;
using StrategyGame.Model.Game;
using StrategyGame.Model.IService;
using StrategyGame.Model.IService.AI;
using StrategyGame.Model.IService.Pathfinding;
using StrategyGame.Model.IService.View;
using StrategyGame.Service.AI;
using StrategyGame.Service.Pathfinding;
using StrategyGame.Service.Sound;
using StrategyGame.Service.View;
using IServiceProvider = StrategyGame.Model.IService.IServiceProvider;

namespace StrategyGame.Service
{
    public class ServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, IService> services;

        public ServiceProvider(GameWindow gameWindow,  Action<string> changeScene)
        {
            services = new Dictionary<Type, IService>();

            AddService(typeof(IGraphicsService), new GraphicsService(gameWindow));
            AddService(typeof(IKeystateService), new KeystateService(gameWindow, this));
            AddService(typeof(ITimeService), new TimeService(gameWindow));
            AddService(typeof(IFileResourceService), new FileResourceService(FileResourceService.GetSourceDirectory() + "/Content"));
            AddService(typeof(IMapService), new MapService(this));
            AddService(typeof(IPathfinderService), new Pathfinder());
            AddService(typeof(IOwnerColorService), new OwnerColorService());
            AddService(typeof(IRandomService), new RandomService());
            AddService(typeof(IGameObjectService), new GameObjectService());
            AddService(typeof(ISceneService), new SceneService(changeScene));
            AddService(typeof(IGameStateService), new GameStateService());
            AddService(typeof(IGoldService), new GoldService());
            AddService(typeof(IAIService), new AIService(this));
            AddService(typeof(IAudioService), new AudioService(this));
        }

        public T GetService<T>()
            where T : IService
        {
            return (T)services[typeof(T)];
        }

        public void Update()
        {
            foreach (var service in services.Values)
            {
                var updatable = service as IUpdatable;
                updatable?.Update();
            }
        }

        internal void AddService(Type type, IService service)
        {
            services[type] = service;
        }
    }
}
