using OpenTK;

namespace StrategyGame.Model
{
    /// <summary>
    /// Interface for entites for the model
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets Id of the entity as <see cref="EntityType"/>
        /// </summary>
        EntityType Id { get; }

        /// <summary>
        /// Gets the position of the entity
        /// </summary>
        Vector2 Position { get; }
    }
}
