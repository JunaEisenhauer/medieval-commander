using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using StrategyGame.Model.IService.View;

namespace StrategyGame.Model.Components
{
    public class DrawTextComponent : IComponent
    {
        public IEntity Parent { get; }

        public int Layer { get; }

        public string Font { get; set; }

        private string text;

        public string Text
        {
            get => text;
            set
            {
                text = value;
                CreateCharacters();
            }
        }

        public Color Color { get; set; }

        private readonly List<IEntity> characters = new List<IEntity>();

        public DrawTextComponent(IEntity parent, int layer, string font, string text, Color color)
        {
            Parent = parent;
            parent?.AddComponent(this);
            Layer = layer;
            Font = font;
            Text = text;
            Color = color;
        }

        public void Update()
        {
            for (int i = 0; i < text.Length; i++)
            {
                Parent.Scene.ServiceProvider.GetService<IGraphicsService>().DrawCharacter(characters[i], Layer, Font, Text[i], Color);
            }
        }

        private void CreateCharacters()
        {
            foreach (var character in characters)
            {
                character.Destroy();
            }

            characters.Clear();

            for (int i = 0; i < text.Length; i++)
            {
                characters.Add(new Entity(Parent.Scene, new Vector2(Parent.Position.X + (i * Parent.Size.X), Parent.Position.Y), Parent.Size, false));
            }
        }
    }
}
