using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
using StrategyGame.Model;

namespace StrategyGame.Controller
{
    /// <summary>
    /// Receives and handels key and mouse inputs
    /// </summary>
    public class KeyState : IKeyboard
    {
        private readonly List<Key> pressedKeys;
        private readonly List<MouseButton> pressedButtons;
        private List<Key> pressedKeysLast;
        private List<MouseButton> pressedButtonsLast;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyState"/> class.
        /// </summary>
        /// <param name="window">The game window of the game</param>
        public KeyState(GameWindow window)
        {
            pressedKeys = new List<Key>();
            pressedKeysLast = new List<Key>();
            pressedButtons = new List<MouseButton>();
            pressedButtonsLast = new List<MouseButton>();

            window.KeyDown += (s, e) => pressedKeys.Add(e.Key);
            window.KeyUp += (s, e) => pressedKeys.Remove(e.Key);

            window.MouseDown += (s, e) => pressedButtons.Add(e.Button);
            window.MouseUp += (s, e) => pressedButtons.Remove(e.Button);

            window.MouseMove += (s, e) => MousePosition =
                        ((new Vector2((float)e.X / window.Width, (window.Height - (float)e.Y) / window.Height) * 2) -
                        Vector2.One) * new Vector2(1 / ((float)window.Height / window.Width), 1f);
        }

        /// <summary>
        /// Gets current mouse position
        /// </summary>
        public Vector2 MousePosition { get; private set; }

        /// <summary>
        /// Update on every frame resetes last pressed keys
        /// </summary>
        public void Update()
        {
            pressedKeysLast = new List<Key>(pressedKeys);
            pressedButtonsLast = new List<MouseButton>(pressedButtons);
        }

        /// <summary>
        /// Check if key is currently down
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>Is key down</returns>
        public bool IsKeyDown(Key key)
        {
            return pressedKeys.Contains(key);
        }

        /// <summary>
        /// Check if key is pressed in this frame
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>Is key pressed</returns>
        public bool IsKeyPressed(Key key)
        {
            return pressedKeys.Contains(key) && !pressedKeysLast.Contains(key);
        }

        /// <summary>
        /// Check if key is released in this frame
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>Is key released</returns>
        public bool IsKeyReleased(Key key)
        {
            return !pressedKeys.Contains(key) && pressedKeysLast.Contains(key);
        }

        /// <summary>
        /// Check if mouse button is currently down
        /// </summary>
        /// <param name="button">The mouse button to check</param>
        /// <returns>Is mouse button down</returns>
        public bool IsButtonDown(MouseButton button)
        {
            return pressedButtons.Contains(button);
        }

        /// <summary>
        /// Check if mouse button is pressed in this frame
        /// </summary>
        /// <param name="button">The mouse button to check</param>
        /// <returns>Is mouse button pressed</returns>
        public bool IsButtonPressed(MouseButton button)
        {
            return pressedButtons.Contains(button) && !pressedButtonsLast.Contains(button);
        }

        /// <summary>
        /// Check if mouse button is released in this frame
        /// </summary>
        /// <param name="button">The mouse button to check</param>
        /// <returns>Is mouse button released</returns>
        public bool IsButtonReleased(MouseButton button)
        {
            return !pressedButtons.Contains(button) && pressedButtonsLast.Contains(button);
        }
    }
}
