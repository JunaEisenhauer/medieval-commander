using System;
using System.Collections.Generic;
using OpenTK;
using StrategyGame.Model.Game;

namespace StrategyGame.Model
{
    /// <summary>
    /// A moveable army on the map
    /// </summary>
    public interface IArmy : IGameObject
    {
        /// <summary>
        /// Gets or sets the path the army is moving
        /// </summary>
        List<Vector2> Path { get; set; }

        /// <summary>
        /// Gets the movment speed of the army
        /// </summary>
        float Speed { get; }

        /// <summary>
        /// Updates the position of the entity when the army is moving
        /// </summary>
        void UpdatePosition();
    }
}
