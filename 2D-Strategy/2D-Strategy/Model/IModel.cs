using System;
using System.Collections.Generic;
using OpenTK;

namespace StrategyGame.Model
{
    /// <summary>
    /// Interface for different types of scenes
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Gets the entites of the model
        /// </summary>
        List<IEntity> Entities { get; }

        Vector2? StartPosition { get; }

        /// <summary>
        /// Update the model for every frame
        /// </summary>
        /// <param name="keyboard">The keyboard for inputs</param>
        /// <param name="time">The delta time since the last frame</param>
        /// <param name="sceneChangeAction">Action callback to change the scene type</param>
        void Update(IKeyboard keyboard, float time, Action<SceneType> sceneChangeAction);

        /// <summary>
        /// Event on scene changed
        /// </summary>
        /// <param name="type">The scene type</param>
        void SceneChangedEvent(SceneType type);
    }
}
