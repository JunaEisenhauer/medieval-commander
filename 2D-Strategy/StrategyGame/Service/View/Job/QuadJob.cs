using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using StrategyGame.Model;

namespace StrategyGame.Service.View.Job
{
    public class QuadJob : IDrawJob
    {
        public IDrawable Drawable { get; }

        public Color Color { get; }

        public QuadJob(IDrawable drawable, Color color)
        {
            Drawable = drawable;
            Color = color;
        }

        public void Draw()
        {
            Vector2 size = Drawable.Size / 2;

            GL.Color4(Color);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(Drawable.Position - size);
            GL.Vertex2(Drawable.Position + new Vector2(-size.X, size.Y));
            GL.Vertex2(Drawable.Position + size);
            GL.Vertex2(Drawable.Position + new Vector2(size.X, -size.Y));
            GL.End();
        }
    }
}
