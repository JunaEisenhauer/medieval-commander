using System.Collections.Generic;
using OpenTK;
using StrategyGame.Model.Game;
using StrategyGame.Model.IService;
using StrategyGame.Service;

namespace StrategyGame.Model
{
    /// <summary>
    /// The Interface of a scene. The Controller will handle and exchange the scenes.
    /// </summary>
    public interface IScene
    {
        /// <summary>
        /// Gets the service provider to access logic and other global elements.
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the list of entities in this scene.
        /// </summary>
        List<IEntity> Entities { get; }

        /// <summary>
        /// Gets the factory for creating entities.
        /// </summary>
        IEntityFactory EntityFactory { get; }

        void Update();

        void Resize(GameWindow gameWindow);

        void OnChange();

        void AddEntity(IEntity entity);

        void RemoveEntity(IEntity entity);
    }
}
