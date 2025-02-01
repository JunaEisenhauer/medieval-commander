using OpenTK;
using OpenTK.Input;
using StrategyGame.Model.Game;

namespace StrategyGame.Model.IService
{
    public interface IKeystateService : IService, IUpdatable
    {
        /// <summary>
        /// Gets current mouse position.
        /// </summary>
        Vector2 MousePosition { get; }

        /// <summary>
        /// Check if key is currently down.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Is key down.</returns>
        bool IsKeyDown(Key key);

        /// <summary>
        /// Check if key is pressed in this frame.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Is key pressed.</returns>
        bool IsKeyPressed(Key key);

        /// <summary>
        /// Check if key is released in this frame.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Is key released.</returns>
        bool IsKeyReleased(Key key);

        /// <summary>
        /// Check if mouse button is currently down.
        /// </summary>
        /// <param name="button">The mouse button to check.</param>
        /// <returns>Is mouse button down.</returns>
        bool IsButtonDown(MouseButton button);

        /// <summary>
        /// Check if mouse button is pressed in this frame.
        /// </summary>
        /// <param name="button">The mouse button to check.</param>
        /// <returns>Is mouse button pressed.</returns>
        bool IsButtonPressed(MouseButton button);

        /// <summary>
        /// Check if mouse button is released in this frame.
        /// </summary>
        /// <param name="button">The mouse button to check.</param>
        /// <returns>Is mouse button released.</returns>
        bool IsButtonReleased(MouseButton button);
    }
}
