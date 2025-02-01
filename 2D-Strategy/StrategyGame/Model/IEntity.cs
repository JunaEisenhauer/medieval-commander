using StrategyGame.Model.Game;

namespace StrategyGame.Model
{
    /// <summary>
    /// The main entity interface. All gameobject are entities with different components and children.
    /// </summary>
    public interface IEntity : IUpdatable, IDrawable
    {
        /// <summary>
        /// Gets the parent scene the entity is in.
        /// </summary>
        IScene Scene { get; }

        void AddChild(IEntity entity);

        void RemoveChild(IEntity entity);

        void AddComponent(IComponent component);

        void RemoveComponent<T>()
            where T : IComponent;

        T GetComponent<T>() 
            where T : IComponent;

        bool HasComponent<T>()
            where T : IComponent;

        void Destroy();
    }
}
