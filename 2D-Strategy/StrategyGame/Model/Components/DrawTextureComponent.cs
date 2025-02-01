using System.Drawing;
using StrategyGame.Model.IService.View;
using StrategyGame.Service.View;

namespace StrategyGame.Model.Components
{
    public class DrawTextureComponent : IComponent
    {
        public IEntity Parent { get; }

        public int Layer { get; }

        public int TextureId { get; set; }

        public string Texture { get; set; }

        public Color Color { get; set; }

        public bool Flipped { get; set; }

        public DrawTextureComponent(IEntity parent, int layer, int textureId)
        {
            Parent = parent;
            parent?.AddComponent(this);
            Layer = layer;
            TextureId = textureId;
            Color = Color.White;
        }

        public DrawTextureComponent(IEntity parent, int layer, string texture)
        {
            Parent = parent;
            parent?.AddComponent(this);
            Layer = layer;
            Texture = texture;
            Color = Color.White;
        }

        public DrawTextureComponent(IEntity parent, int layer, string texture, Color color)
        {
            Parent = parent;
            parent?.AddComponent(this);
            Layer = layer;
            Texture = texture;
            Color = color;
        }

        public void Update()
        {
            if (TextureId != 0)
            {
                Parent.Scene.ServiceProvider.GetService<IGraphicsService>().DrawTexture(Parent, Layer, TextureId);
            }
            else if (Texture != null)
            {
                Parent.Scene.ServiceProvider.GetService<IGraphicsService>().DrawTexture(Parent, Layer, Texture, Color, Flipped);
            }
        }
    }
}
