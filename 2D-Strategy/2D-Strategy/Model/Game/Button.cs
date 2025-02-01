using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace StrategyGame.Model.Game
{
    public class Button : IEntity
    {
        public EntityType Id { get; private set; }

        public Vector2 Position { get; private set; }

        public Vector2 Size { get; private set; }

        public string Text;

        public Button(Vector2 position, Vector2 size, string text)
        {
            Id = EntityType.UIElement;
            Position = position;
            Size = size;
            Text = text;
        }
    }
}
