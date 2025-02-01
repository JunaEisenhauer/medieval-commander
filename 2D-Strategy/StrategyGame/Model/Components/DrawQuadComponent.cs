using System.Drawing;
using StrategyGame.Model.IService.View;
using StrategyGame.Service.View;

namespace StrategyGame.Model.Components
{
    public class DrawQuadComponent : IComponent
    {
        public IEntity Parent { get; }

        public int Layer { get; }

        public Color Color { get; set; }

        private readonly IGraphicsService graphics;

        public DrawQuadComponent(IEntity parent, int layer, Color color)
        {
            Parent = parent;
            parent?.AddComponent(this);
            Layer = layer;
            Color = color;
            graphics = Parent.Scene.ServiceProvider.GetService<IGraphicsService>();
        }

        public void Update()
        {
            graphics.DrawQuad(Parent, Layer, Color);
        }
    }
}
