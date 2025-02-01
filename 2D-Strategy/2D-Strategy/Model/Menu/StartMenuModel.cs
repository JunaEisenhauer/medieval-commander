using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;

namespace StrategyGame.Model.Menu
{
    /// <summary>
    /// The model for the start menu
    /// </summary>
    public class StartMenuModel : IModel
    {
        public List<IEntity> Entities { get; }

        public Vector2? StartPosition { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartMenuModel"/> class.
        /// </summary>
        public StartMenuModel()
        {
            Entities = new List<IEntity>
            {
                new MenuButton(ButtonType.Start, new Vector2(0f, 0f), new Vector2(0.8f, 0.1f)),
                new MenuButton(ButtonType.Exit, new Vector2(0f, -0.3f), new Vector2(0.8f, 0.1f))
            };
            StartPosition = null;
        }

        public void Update(IKeyboard keyboard, float time, Action<SceneType> sceneChangeAction)
        {
            if (keyboard.IsButtonPressed(MouseButton.Left))
            {
                foreach (var entity in Entities)
                {
                    if (!(entity is MenuButton))
                    {
                        continue;
                    }

                    MenuButton button = (MenuButton)entity;

                    Vector2 downLeft = new Vector2(
                        entity.Position.X - (button.Size.X / 2f),
                        entity.Position.Y - (button.Size.Y / 2f));
                    Vector2 upRight = new Vector2(
                        entity.Position.X + (button.Size.X / 2f),
                        entity.Position.Y + (button.Size.Y / 2f));

                    if (downLeft.X < keyboard.MousePosition.X && upRight.X > keyboard.MousePosition.X && downLeft.Y < keyboard.MousePosition.Y && upRight.Y > keyboard.MousePosition.Y)
                    {
                        if (((MenuButton)entity).Type == ButtonType.Start)
                        {
                            sceneChangeAction.Invoke(SceneType.Game);
                        }

                        if (((MenuButton)entity).Type == ButtonType.Exit)
                        {
                            sceneChangeAction.Invoke(SceneType.Exit);
                        }

                        break;
                    }
                }
            }
        }

        public void SceneChangedEvent(SceneType type)
        {
        }
    }
}
