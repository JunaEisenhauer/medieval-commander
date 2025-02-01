using StrategyGame.Model.Game;

namespace StrategyGame.Model.IService.AI
{
    /// <summary>
    /// Interface of <see cref="IAIService"/>.
    /// </summary>
    public interface IAIService : IService, IUpdatable
    {
        /// <summary>
        /// Function to register an entity as relevant for the behavior of the enemy.
        /// </summary>
        /// <param name="entity">entity that is relevant.</param>
        void Register(IEntity entity);
    }
}
