using OpenTK;

namespace StrategyGame.Model.Menu
{
    /// <summary>
    /// A clickable menu button
    /// </summary>
    public class MenuButton : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuButton"/> class.
        /// </summary>
        /// <param name="type">The type of the button</param>
        /// <param name="position">The position of the button</param>
        /// <param name="size">The size of the button</param>
        public MenuButton(ButtonType type, Vector2 position, Vector2 size)
        {
            Type = type;
            Id = type == ButtonType.Start ? EntityType.MenuStart : EntityType.MenuExit;
            Position = position;
            Size = size;
        }

        public EntityType Id { get; }

        public Vector2 Position { get; }

        public Vector2 Size { get; }

        /// <summary>
        /// Gets the type of button
        /// </summary>
        public ButtonType Type { get; }
    }
}
