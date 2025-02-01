using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
using StrategyGame.Model.IService;
using IServiceProvider = StrategyGame.Model.IService.IServiceProvider;

namespace StrategyGame.Service
{
    public class KeystateService : IKeystateService
    {
        private readonly List<Key> pressedKeys;
        private readonly List<MouseButton> pressedButtons;
        private List<Key> pressedKeysLast;
        private List<MouseButton> pressedButtonsLast;

        /// <summary>
        /// Gets current mouse position.
        /// </summary>
        public Vector2 MousePosition { get; private set; }

        public KeystateService(GameWindow gameWindow, IServiceProvider serviceProvider)
        {
            pressedKeys = new List<Key>();
            pressedKeysLast = new List<Key>();
            pressedButtons = new List<MouseButton>();
            pressedButtonsLast = new List<MouseButton>();

            gameWindow.KeyDown += (s, e) => pressedKeys.Add(e.Key);

            gameWindow.KeyUp += (s, e) => pressedKeys.RemoveAll(k => k == e.Key);

            gameWindow.MouseDown += (s, e) => pressedButtons.Add(e.Button);
            gameWindow.MouseUp += (s, e) => pressedButtons.RemoveAll(b => b == e.Button);

            gameWindow.MouseMove += (s, e) =>
            {
                MousePosition = new Vector2((float)e.X / gameWindow.Width, (float)e.Y / gameWindow.Height);
                MousePosition *= 2;
                MousePosition -= Vector2.One;
                MousePosition *= new Vector2(1f, -1f);

                MousePosition = ToWorldCoordinate(gameWindow, serviceProvider, MousePosition);
            };
        }

        public void Update()
        {
            pressedKeysLast = new List<Key>(pressedKeys);
            pressedButtonsLast = new List<MouseButton>(pressedButtons);
        }

        /// <summary>
        /// Check if key is currently down.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Is key down.</returns>
        public bool IsKeyDown(Key key)
        {
            return pressedKeys.Contains(key);
        }

        /// <summary>
        /// Check if key is pressed in this frame.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Is key pressed.</returns>
        public bool IsKeyPressed(Key key)
        {
            return pressedKeys.Contains(key) && !pressedKeysLast.Contains(key);
        }

        /// <summary>
        /// Check if key is released in this frame.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Is key released.</returns>
        public bool IsKeyReleased(Key key)
        {
            return !pressedKeys.Contains(key) && pressedKeysLast.Contains(key);
        }

        /// <summary>
        /// Check if mouse button is currently down.
        /// </summary>
        /// <param name="button">The mouse button to check.</param>
        /// <returns>Is mouse button down.</returns>
        public bool IsButtonDown(MouseButton button)
        {
            return pressedButtons.Contains(button);
        }

        /// <summary>
        /// Check if mouse button is pressed in this frame.
        /// </summary>
        /// <param name="button">The mouse button to check.</param>
        /// <returns>Is mouse button pressed.</returns>
        public bool IsButtonPressed(MouseButton button)
        {
            return pressedButtons.Contains(button) && !pressedButtonsLast.Contains(button);
        }

        /// <summary>
        /// Check if mouse button is released in this frame.
        /// </summary>
        /// <param name="button">The mouse button to check.</param>
        /// <returns>Is mouse button released.</returns>
        public bool IsButtonReleased(MouseButton button)
        {
            return !pressedButtons.Contains(button) && pressedButtonsLast.Contains(button);
        }

        private Vector2 ToWorldCoordinate(GameWindow gameWindow, IServiceProvider serviceProvider, Vector2 position)
        {
            var mapService = serviceProvider.GetService<IMapService>();

            if (gameWindow.Height > gameWindow.Width)
            {
                position *= new Vector2(1f, 1 / ((float)gameWindow.Width / gameWindow.Height));
            }
            else
            {
                position *= new Vector2(1 / ((float)gameWindow.Height / gameWindow.Width), 1f);
            }

            if (!serviceProvider.GetService<IGameStateService>().Running)
            {
                var windowAspect = (float)gameWindow.Width / gameWindow.Height;

                if (windowAspect > 1)
                {
                    position *= new Vector2((float)gameWindow.Height / gameWindow.Width, -1f);
                }
                else
                {
                    position *= new Vector2(1f, -(float)gameWindow.Width / gameWindow.Height);
                }
            }
            else
            {
                var windowAspect = (float)gameWindow.Width / gameWindow.Height;
                var mapAspect = (float)mapService.Width /
                                mapService.Height;
                if (windowAspect > 1)
                {
                    position -= new Vector2(-windowAspect, 1);
                    position *= new Vector2(1 / (Math.Min(windowAspect, mapAspect) / mapService.Width), 1 / (Math.Min(windowAspect, mapAspect) / mapService.Width));
                }
                else
                {
                    position -= new Vector2(-1, 1 / windowAspect);
                    position *= new Vector2(1 / (Math.Min(1 / windowAspect, 1 / mapAspect) / mapService.Height), 1 / (Math.Min(1 / windowAspect, 1 / mapAspect) / mapService.Height));
                }

                position *= new Vector2(1 / 2f, 1 / -2f);

                var widthDiff = (windowAspect * mapService.Height) - mapService.Width;
                var heightDiff = ((1 / windowAspect) * mapService.Width) - mapService.Height;
                position -= new Vector2(Math.Max(0f, widthDiff / 2), Math.Max(0f, heightDiff / 2));
            }

            return position;
        }
    }
}
