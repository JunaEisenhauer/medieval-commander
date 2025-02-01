namespace StrategyGame.Model
{
    /// <summary>
    /// The Interface for components. Components defines attributes of a gameobject.
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// Gets the parent of the component.
        /// </summary>
        IEntity Parent { get; }

        void Update();
    }
}
